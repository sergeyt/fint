module Fint.MetaReader

open System
open System.IO
open System.Collections.Generic
open Fint.Enums
open Fint.Meta
open Fint.PEImage
open Fint.IO
open Fint.MethodBody

let tryGet (d : IDictionary<'k, 'v>) (key : 'k) (init: unit -> 'v) =
    let setup =
        let v = init()
        d.[key] <- v
        v
    let mutable value = Unchecked.defaultof<'v>
    if d.TryGetValue(key, &value) then value
    else setup

let MetaReader(reader : BinaryReader) =
    let image = ReadExecutableHeaders(reader)
    // goto metadata
    let startOffset = ResolveVirtualAddress(image.Sections, image.Metadata.VirtualAddress)
    Move(reader, startOffset)
    // Metadata Header
    // Signature
    if (reader.ReadUInt32() <> 0x424A5342u) then
        invalidOp "invalid metadata header"
    // MajorVersion: 2
    // MinorVersion: 2
    // Reserved: 4
    Skip(reader, 8)
    let runtimeVersion = ReadZeroTerminatedString(reader, reader.ReadInt32())
    // align for dword boundary
    Align4(reader)
    // Flags: 2
    Skip(reader, 2)
    // heap headers
    let heapHeader _ = 
        let offset = startOffset + int64 (reader.ReadUInt32())
        let size = int(reader.ReadUInt32())
        let name = ReadAlignedString(reader, 16)
        {| offset = offset; size = size; name = name; |}
    let streamNum = int (reader.ReadUInt16());
    let streams = [ 1 .. streamNum ] |> List.map heapHeader

    // heap locations
    let tableStream = streams |> List.find (fun t -> t.name = "#-" || t.name = "#~")
    let stringStream = streams |> List.tryFind (fun t -> t.name = "#Strings")
    let usStream = streams |> List.tryFind (fun t -> t.name = "#US") // user strings
    let guidStream = streams |> List.tryFind (fun t -> t.name = "#GUID")
    let blobStream = streams |> List.tryFind (fun t -> t.name = "#Blob")

    // read headers of metadata tables
    Move(reader, tableStream.offset)
    // Reserved: 4, always 0
    // MajorVersion: 1
    // MinorVersion: 1
    Skip(reader, 6)
    let heapSizes = int (reader.ReadByte())
    Skip(reader, 1) //reserved: 1, always 1
    let validTables = reader.ReadUInt64()
    let sortedTables = reader.ReadUInt64()

    let stringIndexSize = if (heapSizes &&& 1) = 0 then 2 else 4
    let guidIndexSize = if (heapSizes &&& 2) = 0 then 2 else 4
    let blobIndexSize = if (heapSizes &&& 4) = 0 then 2 else 4

    // read table row counts and calculate table size
    let readRowCount i =
        // if bit is set table is presented, otherwise it is empty
        if (((validTables >>> i) &&& 1UL) = 0UL) then None
        else Some (reader.ReadInt32())

    let rowCounts = [| 0 .. 63 |] |> Array.map readRowCount
    let rowCount id =
        match rowCounts.[int id] with
            | None -> 0
            | Some t -> t
    let sizeOfIndex n = if n >= 0x10000 then 4 else 2
    let mutable codedIndexSizes = new Dictionary<CodedIndexId, int>()
    let codedIndexSize i =
        let calc() =
            let maxRowCount = i.tables |> Array.map rowCount |> Array.max
            sizeOfIndex (maxRowCount <<< i.bits)
        tryGet codedIndexSizes i.id calc

    let colSize (col: Column) =
        match col.value with
            | Int16 _ -> 2
            | Int32 _ -> 4
            | StringIndex _ -> stringIndexSize
            | GuidIndex _ -> guidIndexSize
            | BlobIndex _ -> blobIndexSize
            | TableIndex i -> sizeOfIndex (rowCount i.table)
            | CodedIndex i -> codedIndexSize i

    let mutable tablePos = GetPosition(reader)
    let makeTable i =
        let createTable rowCount =
            let id: TableId = enum i
            let offset = tablePos
            let cols = Schema.TableColumns id |> Array.map (fun c -> {
                name = c.name;
                value = c.value;
                size = colSize(c);
            })
            let rowSize = cols |> Array.sumBy (fun c -> c.size)
            let tableSize = rowSize * rowCount
            let table: Table = {
                id = id;
                offset = offset;
                rowCount = rowCount;
                rowSize = rowSize;
                size = tableSize;
                isSorted = ((sortedTables >>> i) &&& 1UL) <> 0UL;
                columns = cols;
            }
            tablePos <- tablePos + int64 tableSize
            table

        match rowCounts.[i] with
            | None -> None
            | Some t -> Some (createTable t)

    let tables = [ 0 .. 63 ] |> List.map makeTable

    let readCell (col: ComputedColumn) =
        let readStr i =
            Move(reader, stringStream.Value.offset + int64 i)
            ReadUTF8(reader, -1)

        let readGuid i =
            let guids = lazy (
                let heap = guidStream.Value
                seq {
                    Move(reader, heap.offset)
                    let mutable size = heap.size
                    while size > 0 do
                        yield Guid(ReadBytes(reader, 16))
                        size <- size - 16
                } |> Seq.toArray
            )
            if i = 0 then Guid.Empty // guid index is 1 based
            else guids.Value.[i - 1]

        let readBlob i =
            Move(reader, blobStream.Value.offset + int64 i)
            let size = ReadPackedInt(reader)
            ReadBytes(reader, size)

        let idxReader i fetch =
            let result () =
                Restore(reader, (fun () -> fetch i))
            result

        let value = if col.size = 2 then int(reader.ReadUInt16()) else int(reader.ReadUInt32())
        match col.value with
            | Int16 _ -> Int16Cell(int16 value)
            | Int32 _ -> Int32Cell(int32 value)
            | StringIndex _ -> StringCell(idxReader value readStr)
            | GuidIndex _ -> GuidCell(idxReader value readGuid)
            | BlobIndex _ -> BlobCell(idxReader value readBlob)
            | TableIndex t -> TableIndexCell({table=t.table;index=int value;})
            | CodedIndex i -> TableIndexCell(decodeCodedIndex(i, uint32 value))

    let findTable tableId =
        match tables.[int tableId] with
            | None -> invalidArg "tableId" (sprintf "table %A does not exist" tableId)
            | Some t -> t

    let seekRow table idx =
        let offset = table.offset + int64 (idx * table.rowSize)
        Move(reader, offset)

    // metadata reader API
    let readRow (tableId: TableId) (idx: int) =
        let table = findTable tableId
        if (idx < 0 || idx >= table.rowCount)
        then raise <| ArgumentOutOfRangeException("idx")
        seekRow table idx
        let cells = table.columns |> Array.map readCell
        cells

    let moveToRVA (rva: uint32) =
        let offset = ResolveVirtualAddress(image.Sections, rva)
        Move(reader, offset)
        reader

    let cellInt32 cell =
        match cell with
            | Int32Cell t -> t
            | Int16Cell t -> int t
            | _ -> invalidOp "expected int32 or int16 cell"

    let cellStr cell =
        match cell with
            | StringCell t -> t()
            | _ -> invalidOp "expected string cell"

    let noneFn() = None
    // TODO cache method bodies
    let readBodyAt rva = readMethodBody(moveToRVA(rva))

    let readMethod idx =
        let row = readRow TableId.MethodDef idx
        let rva = uint32(cellInt32(row.[Schema.MethodDef.RVA.index]))
        let name = cellStr(row.[Schema.MethodDef.Name.index])
        let bodyReader(rva: uint32) =
            match rva with
            | 0u -> noneFn
            | t -> (fun () -> Some (readBodyAt t))
        let method: MethodDef = {
            rva=rva;
            name=name;
            body=bodyReader(rva);
        }
        method

    let readEntryPoint() =
        let p = decodeTableIndex(image.EntryPointToken)
        if p.table = TableId.MethodDef then Some (readMethod p.index)
        else None

    let dump() =
        let dumpCell col =
            let cell = readCell col
            match cell with
                | Int16Cell t -> t :> obj
                | Int32Cell t -> t :> obj
                | StringCell t -> t() :> obj
                | GuidCell t -> t() :> obj
                | BlobCell t -> sprintf "BLOB[%d]" (t().Length) :> obj
                | TableIndexCell t -> sprintf "%A(%d)" t.table t.index :> obj
        let dumpTable table =
            let dumpRow idx =
                seekRow table idx
                let cells = table.columns |> Array.map (fun c -> {|name=c.name;value=dumpCell c;|})
                cells
            let rows = [| 0 .. table.rowCount - 1 |] |> Array.map dumpRow
            {|table=table.id; tableId=int table.id; rows=rows|}
        tables |> List.filter (fun t -> t.IsSome) |> List.map (fun t -> dumpTable t.Value)

    {|
        image = image;
        rowCount = rowCount;
        tables = tables;
        readRow = readRow;
        readMethod = readMethod;
        readEntryPoint = readEntryPoint;
        dump = dump
    |}

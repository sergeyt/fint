module Fint.MetaReader

open System
open System.IO
open System.Collections.Generic
open Fint.Enums
open Fint.Meta
open Fint.PEImage
open Fint.IO

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
        raise (invalidOp ("invalid metadata header"))
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
        {|
        offset = startOffset + int64 (reader.ReadUInt32());
        size = int(reader.ReadUInt32());
        name = ReadAlignedString(reader, 16);
        |}
    let heapNum = int (reader.ReadUInt16());
    let heapHeaders = [ 1 .. heapNum ] |> List.map heapHeader

    // heap locations
    let tableAddr = heapHeaders |> List.find (fun t -> t.name = "#-" || t.name = "#~")
    let strAddr = heapHeaders |> List.tryFind (fun t -> t.name = "#Strings")
    let usAddr = heapHeaders |> List.tryFind (fun t -> t.name = "#US") // user strings
    let guidAddr = heapHeaders |> List.tryFind (fun t -> t.name = "#GUID")
    let blobAddr = heapHeaders |> List.tryFind (fun t -> t.name = "#Blob")

    // read headers of metadata tables
    Move(reader, tableAddr.offset)
    Skip(reader, 4) //reserved: 4, always 0

    let metaHeader = {|
        MajorVersion = reader.ReadByte();
        MinorVersion = reader.ReadByte();
        HeapSizes = int (reader.ReadByte());
        Reserved = reader.ReadByte(); //reserved: 1, always 1
        Valid = reader.ReadUInt64();
        Sorted = reader.ReadUInt64();
    |}

    let stringIndexSize = if (metaHeader.HeapSizes &&& 1) = 0 then 2 else 4
    let guidIndexSize = if (metaHeader.HeapSizes &&& 2) = 0 then 2 else 4
    let blobIndexSize = if (metaHeader.HeapSizes &&& 4) = 0 then 2 else 4

    // read table row counts and calculate table size
    let readRowCount i =
        let readOne i =
            let id: TableId = enum i 
            {|
                id = id;
                rowCount = reader.ReadInt32();
                isSorted = ((metaHeader.Sorted >>> i) &&& 1UL) <> 0UL;
            |}
        // if bit is set table is presented, otherwise it is empty
        if (((metaHeader.Valid >>> i) &&& 1UL) = 0UL) then None
        else Some (readOne i)

    let rowCounts = [ 0 .. 63 ] |> List.map readRowCount
    let rowCount id =
        match rowCounts.[int id] with
            | None -> 0
            | Some t -> t.rowCount
    let sizeOfIndex n = if n >= 0x10000 then 4 else 2
    let mutable codedIndexSizes = dict []
    let codedIndexSize i =
        let calc() =
            let maxRowCount = i.tables |> Seq.ofArray |> Seq.map rowCount |> Seq.max
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
        let createTable id rowCount isSorted =
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
                isSorted = isSorted;
                columns = cols;
            }
            tablePos <- tablePos + int64 tableSize
            table

        match rowCounts.[i] with
            | None -> None
            | Some t -> Some (createTable t.id t.rowCount t.isSorted)

    let tables = [ 0 .. 63 ] |> List.map makeTable

    let readCell (col: ComputedColumn) =
        let readStr i =
            Move(reader, strAddr.Value.offset + int64 i)
            ReadUTF8(reader, -1)

        let readGuid i =
            let guids = lazy (
                let heap = guidAddr.Value
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
            Move(reader, blobAddr.Value.offset + int64 i)
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
            | None -> invalidArg "table" (sprintf "table %A does not exist" tableId)
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

    let dump() =
        let dumpTable table = 
            let dumpRow idx =
                seekRow table idx
                let cells = table.columns |> Array.map (fun c -> {|name=c.name;value=readCell c;|})
                cells
            let rows = [ 0 .. table.rowCount - 1 ] |> List.map dumpRow
            {|table=table.id; rows=rows|}
        tables |> List.filter (fun t -> t.IsSome) |> List.map (fun t -> dumpTable t.Value)

    {|
        image = image;
        tables = tables;
        readRow = readRow;
        dump = dump
    |}

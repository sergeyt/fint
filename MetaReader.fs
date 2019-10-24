module Fint.MetaReader

open System
open System.IO
open System.Collections.Generic
open Fint.Enums
open Fint.Types
open Fint.PEImage
open Fint.IO
open Fint.MethodBody
open Fint.Utils
open Fint.CodedIndex
open Fint.Signature

let tryGet (d : IDictionary<'k, 'v>) (key : 'k) (init: unit -> 'v) =
    match d.TryGetValue key with
        | true, v -> v
        | false, _ -> let v = init()
                      d.Add(key, v)
                      v

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

    let readString i =
        Move(reader, stringStream.Value.offset + int64 i)
        ReadUTF8(reader, -1)

    let readUserString i =
        Move(reader, usStream.Value.offset + int64 i)
        let mutable len = ReadPackedInt(reader)
        let buf = ReadBytes(reader, len)
        len <- if (int buf.[len - 1] = 0 || int buf.[len - 1] = 1) then len - 1 else len
        System.Text.Encoding.Unicode.GetString(buf, 0, len)

    let readCell (col: ComputedColumn) =
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
            | StringIndex _ -> StringCell(idxReader value readString)
            | GuidIndex _ -> GuidCell(idxReader value readGuid)
            | BlobIndex _ -> BlobCell(idxReader value readBlob)
            | TableIndex t -> TableIndexCell({table=t.table;index=int value;})
            | CodedIndex i -> TableIndexCell(decodeCodedIndex(i, uint32 value))

    let findTable tableId =
        match tables.[int tableId] with
            | None -> invalidArg "tableId" (sprintf "table %A does not exist" tableId)
            | Some t -> t

    let seekRow (table: Table) idx =
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

    let dumpCell cell =
        match cell with
            | Int16Cell t -> t.ToString()
            | Int32Cell t -> t.ToString()
            | StringCell t -> t()
            | GuidCell t -> t().ToString()
            | BlobCell t -> sprintf "BLOB[%d]" (t().Length)
            | TableIndexCell t -> sprintf "%A[%d]" t.table t.index
    let dump() =
        let dumpTable table =
            let dumpRow idx =
                seekRow table idx
                let cells = table.columns |> Array.map readCell
                Array.zip table.columns cells |> Array.map (fun (c, v) -> sprintf "%s=%s" c.name (dumpCell v))
            let rows = [| 0 .. table.rowCount - 1 |] |> Array.map dumpRow
            {|table=table.id; tableId=int table.id; rows=rows|}
        tables |> List.filter (fun t -> t.IsSome) |> List.map (fun t -> dumpTable t.Value)

    let moveToRVA (rva: uint32) =
        let offset = ResolveVirtualAddress(image.Sections, rva)
        Move(reader, offset)
        reader

    let cellInt32 cell =
        match cell with
            | Int32Cell t -> t
            | Int16Cell t -> int t
            | _ -> invalidOp "expect int32 or int16 cell"

    let cellStr cell =
        match cell with
            | StringCell t -> t()
            | _ -> invalidOp "expect string cell"

    let cellBlob cell =
        match cell with
            | BlobCell t -> t()
            | _ -> invalidOp "expect blob cell"

    let cellIdx cell =
        match cell with
            | TableIndexCell t -> t
            | _ -> invalidOp "expect index cell"

    let noneFn() = None
    let readBodyImpl rva = readMethodBody(moveToRVA(rva))
    let readBodyAt = memoize readBodyImpl

    let resolveLocalVarsImpl (token: uint32) =
        let readVars() =
            let t = decodeTableIndex token
            let row = readRow t.table (t.index - 1)
            let blob = cellBlob(row.[Schema.StandAloneSig.Signature.index])
            let reader = MakeReader(blob)
            let prolog = ReadPackedInt(reader)
            if prolog <> int SignatureKind.LocalVars
            then failwith "Invalid local variable signature."
            let count = ReadPackedInt(reader)
            let makeVar i =
                let v: LocalVar = {
                    Index=i;
                    Type=decodeTypeSignature(reader);
                    Name=sprintf "v%d" i;
                }
                v
            let vars = [|0..count-1|] |> Array.map makeVar
            vars
        match token with
        | 0u -> [||]
        | _ -> readVars()
    let resolveLocalVars = memoize resolveLocalVarsImpl

    let makeMethod(row: Cell array) =
        let rva = uint32(cellInt32(row.[Schema.MethodDef.RVA.index]))
        let name = cellStr(row.[Schema.MethodDef.Name.index])
        let flags: MethodAttributes = enum (cellInt32(row.[Schema.MethodDef.Flags.index]))
        let sigBlog = cellBlob(row.[Schema.MethodDef.Signature.index])
        let signature = decodeMethodSignature(MakeReader(sigBlog))
        let bodyReader(rva: uint32) =
            match rva with
            | 0u -> noneFn
            | t -> (fun () -> Some (readBodyAt t))
        let body = bodyReader(rva)
        let method: MethodDef = {
            rva=rva;
            name=name;
            flags=flags;
            signature=signature;
            body=body;
            localVars=(fun () ->
                match body() with
                | None -> [||]
                | Some body -> resolveLocalVars body.localSig)
        }
        method

    let readMethod idx =
        let row = readRow TableId.MethodDef idx
        makeMethod row

    let readEntryPoint() =
        let p = decodeTableIndex(image.EntryPointToken)
        if p.table = TableId.MethodDef then Some (readMethod (p.index - 1))
        else None

    let resolveToken (token: uint32) =
        let msb = int(token >>> 24)
        let index = int(token &&& 0xffffffu)
        match msb with
        | 0x70 -> StringToken(readUserString index)
        | _ -> let tableId: TableId = enum msb
               let cells = readRow tableId (index - 1)
               RowToken({table=tableId; cells=cells})

    let makeTypeDef (row: Cell array) =
        let ns = cellStr(row.[Schema.TypeDef.TypeNamespace.index])
        let name = cellStr(row.[Schema.TypeDef.TypeName.index])
        let result: TypeDef = {
            ns=ns;
            name=name;
        }
        result

    let makeTypeRef (row: Cell array) =
        let ns = cellStr(row.[Schema.TypeRef.TypeNamespace.index])
        let name = cellStr(row.[Schema.TypeRef.TypeName.index])
        let result: TypeRef = {
            ns=ns;
            name=name;
        }
        result

    let resolveMemberRefParent (row: Cell array) =
        let p = cellIdx(row.[Schema.MemberRef.Class.index])
        let row = readRow p.table (p.index - 1)
        match p.table with
        | TableId.TypeDef -> TypeDefParent(makeTypeDef row)
        | TableId.TypeRef -> TypeRefParent(makeTypeRef row)
        | _ -> failwith "expect TypeDef or TypeRef"

    let makeMemberRef (row: Cell array) =
        let parent = resolveMemberRefParent(row)
        let sigBlob = cellBlob(row.[Schema.MemberRef.Signature.index])
        let signature = decodeMethodSignature(MakeReader(sigBlob))
        let name = cellStr(row.[Schema.MemberRef.Name.index])
        let result: MemberRef = {
            parent=parent;
            name=name;
            signature=signature;
        }
        result

    {|
        image = image;
        rowCount = rowCount;
        tables = tables;
        readRow = readRow;
        readMethod = readMethod;
        readEntryPoint = readEntryPoint;
        readString = readString;
        resolveToken = resolveToken;
        makeMethod = makeMethod;
        makeTypeRef = makeTypeRef;
        makeMemberRef = makeMemberRef;
        dump = dump;
        findTable = findTable;
        dumpCell = dumpCell;
    |}

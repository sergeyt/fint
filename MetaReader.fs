module Fint.MetaReader

open System.IO
open Fint.Meta
open Fint.PEImage
open Fint.IO

let ReadTables(reader : BinaryReader, image : PEImage) =
    // goto metadata
    let startOffset = ResolveVirtualAddress(image.Sections, image.Metadata.VirtualAddress)
    reader.BaseStream.Position <- startOffset
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
        size = reader.ReadUInt32();
        name = ReadAlignedString(reader, 16);
        |}
    let heapHeaders = [ 1 .. int (reader.ReadUInt16()) ] |> List.map heapHeader
    heapHeaders

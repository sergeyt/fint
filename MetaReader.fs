module Fint.MetaReader

open System.IO
open Fint.Meta
open Fint.PEImage
open Fint.IO

let ReadTables(reader : BinaryReader, image : PEImage) =
    // goto metadata
    reader.BaseStream.Position <- ResolveVirtualAddress
                                      (image.Sections,
                                       image.Metadata.VirtualAddress)
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

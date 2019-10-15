module Fint.PEImage

open System
open System.IO
open Fint.Enums
open Fint.IO

type Section =
    { Name : string
      VirtualSize : uint32
      VirtualAddress : uint32
      SizeOfRawData : uint32
      PointerToRawData : uint32
      PointerToRelocations : uint32
      PointerToLineNumbers : uint32
      NumberOfRelocations : uint16
      NumberOfLineNumbers : uint16
      Characteristics : uint32 }

// Translates Relative Virtual Address (RVA) to the actual Virtual Address (VA)
let ResolveVirtualAddress(sections : Section list, rva : uint32) =
    let inSection (s : Section) =
        rva >= s.VirtualAddress && rva < s.VirtualAddress + s.SizeOfRawData
    let s = sections |> List.find inSection
    (int64) (s.PointerToRawData + rva - s.VirtualAddress)

type DataDirectory =
    { VirtualAddress : uint32
      Size : uint32 }

let IsEmptyDirectory(d : DataDirectory) = d.VirtualAddress = 0u && d.Size = 0u

type PEImage =
    { Architecture : TargetArchitecture
      ModuleKind : ModuleKind
      Characteristics : ModuleCharacteristics
      Attributes : ModuleAttributes
      EntryPointToken : uint32
      Sections : Section list
      Metadata : DataDirectory
      Debug : DataDirectory
      Resources : DataDirectory
      StrongName : DataDirectory }


let ReadArchitecture(reader : BinaryReader) =
    let target : TargetArchitecture = enum (int (reader.ReadUInt16()))
    if not (Enum.IsDefined(typeof<TargetArchitecture>, target)) then
        invalidOp "bad architecture"
    target

let ReadDataDirectory(reader : BinaryReader) =
    let d : DataDirectory =
        { VirtualAddress = reader.ReadUInt32()
          Size = reader.ReadUInt32() }
    d

let ResolveModuleKind(characteristics : uint16, subsystem : uint16) =
    if (characteristics &&& uint16 0x2000) <> uint16 0 then ModuleKind.Dll
    elif (subsystem = uint16 0x2 || subsystem = uint16 0x9) then
        ModuleKind.Windows
    else ModuleKind.Console

let ReadSection(reader : BinaryReader) =
    let result : Section =
        { Name = ReadZeroTerminatedString(reader, 8)
          VirtualSize = reader.ReadUInt32()
          VirtualAddress = reader.ReadUInt32()
          SizeOfRawData = reader.ReadUInt32()
          PointerToRawData = reader.ReadUInt32()
          PointerToRelocations = reader.ReadUInt32()
          PointerToLineNumbers = reader.ReadUInt32()
          NumberOfRelocations = reader.ReadUInt16()
          NumberOfLineNumbers = reader.ReadUInt16()
          Characteristics = reader.ReadUInt32() }
    result

let ReadSections(reader : BinaryReader, count : int) =
    seq {
        for _ in 1..count -> ReadSection(reader)
    }
    |> List.ofSeq

let ReadExecutableHeaders(reader : BinaryReader) =
    // - DOSHeader
    // PE: 2
    // Start: 58
    // Lfanew: 4
    // End: 64
    if reader.ReadUInt16() <> uint16 0x5a4d then invalidOp "bad DOS magic"
    Skip(reader, 58)
    Move(reader, int64 (reader.ReadUInt32()))
    // PE NT signature
    if reader.ReadUInt32() <> 0x00004550u then invalidOp "bad NT magic"
    // - PEFileHeader
    let architecture = ReadArchitecture(reader) // 2 bytes
    let numberOfSections = reader.ReadUInt16()
    // TimeDateStamp: 4
    // PointerToSymbolTable: 4
    // NumberOfSymbols: 4
    // OptionalHeaderSize: 2
    Skip(reader, 14)
    // Characteristics: 2
    let characteristics = reader.ReadUInt16()
    // - PEOptionalHeader
    //   - StandardFieldsHeader
    // Magic: 2
    let pe64 = reader.ReadUInt16() = uint16 0x20b // pe32 || pe64
    // LMajor: 1
    // LMinor: 1
    // CodeSize: 4
    // InitializedDataSize: 4
    // UninitializedDataSize: 4
    // EntryPointRVA: 4
    // BaseOfCode: 4
    // BaseOfData: 4 || 0
    //   - NTSpecificFieldsHeader
    // ImageBase: 4 || 8
    // SectionAlignment: 4
    // FileAlignement: 4
    // OSMajor: 2
    // OSMinor: 2
    // UserMajor: 2
    // UserMinor: 2
    // SubSysMajor: 2
    // SubSysMinor: 2
    // Reserved: 4
    // ImageSize: 4
    // HeaderSize: 4
    // FileChecksum: 4
    Skip(reader, 66)
    // SubSystem: 2
    let subsystem = reader.ReadUInt16()
    // DLLFlags: 2
    let dllCharacteristics = reader.ReadUInt16()
    // StackReserveSize: 4 || 8
    // StackCommitSize: 4 || 8
    // HeapReserveSize: 4 || 8
    // HeapCommitSize: 4 || 8
    // LoaderFlags: 4
    // NumberOfDataDir: 4
    //   - DataDirectoriesHeader
    // ExportTable: 8
    // ImportTable: 8
    // ResourceTable: 8
    // ExceptionTable: 8
    // CertificateTable: 8
    // BaseRelocationTable: 8
    Skip(reader,
          (if pe64 then 88
           else 72))
    // Debug: 8
    let debug = ReadDataDirectory(reader)
    // Copyright: 8
    // GlobalPtr: 8
    // TLSTable: 8
    // LoadConfigTable: 8
    // BoundImport: 8
    // IAT: 8
    // DelayImportDescriptor: 8
    Skip(reader, 56)
    // CLIHeader: 8
    let cliHeader = ReadDataDirectory(reader)
    if IsEmptyDirectory(cliHeader) then invalidOp ("not a CLI image")
    // Reserved: 8
    Skip(reader, 8)
    let moduleKind = ResolveModuleKind(characteristics, subsystem)
    let characteristics : ModuleCharacteristics = enum (int dllCharacteristics)
    let sections = ReadSections(reader, int numberOfSections)
    // reader CLI header
    Move(reader, ResolveVirtualAddress(sections, cliHeader.VirtualAddress))
    // CLIHeader
    // Cb: 4
    // MajorRuntimeVersion: 2
    // MinorRuntimeVersion: 2
    Skip(reader, 8)
    let metadata = ReadDataDirectory(reader)
    let attributes : ModuleAttributes = enum (int (reader.ReadUInt32()))
    // EntryPointToken: 4
    let entryPoint = reader.ReadUInt32()
    // Resources: 8
    let resources = ReadDataDirectory(reader)
    // StrongNameSignature: 8
    let strongName = ReadDataDirectory(reader)

    let result : PEImage =
        { Architecture = architecture
          ModuleKind = moduleKind
          Characteristics = characteristics
          Attributes = attributes
          EntryPointToken = entryPoint
          Sections = sections
          Metadata = metadata
          Debug = debug
          Resources = resources
          StrongName = strongName }
    result

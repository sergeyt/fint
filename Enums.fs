module Fint.Enums

open System

type TargetArchitecture =
    | I386 = 0x014c
    | AMD64 = 0x8664
    | IA64 = 0x0200
    | ARMv7 = 0x01c4

type ModuleKind =
    | Dll = 0
    | Windows = 1
    | Console = 2

[<FlagsAttribute>]
type ModuleAttributes =
    | ILOnly = 1
    | Required32Bit = 2
    | StrongNameSigned = 8
    | Preferred32Bit = 0x00020000

[<FlagsAttribute>]
type ModuleCharacteristics =
    | HighEntropyVA = 0x0020
    | DynamicBase = 0x0040
    | NoSEH = 0x0400
    | NXCompat = 0x0100
    | AppContainer = 0x1000
    | TerminalServerAware = 0x8000

// Defines metadata table codes
type TableId =
    | Assembly = 0x20
    | AssemblyOS = 0x22
    | AssemblyProcessor = 0x21
    | AssemblyRef = 0x23
    | AssemblyRefOS = 0x25
    | AssemblyRefProcessor = 0x24
    | ClassLayout = 0x0F
    | Constant = 0x0B
    | CustomAttribute = 0x0C
    | DeclSecurity = 0x0E
    | EventMap = 0x12
    | Event = 0x14
    | ExportedType = 0x27
    | Field = 0x04
    | FieldLayout = 0x10
    | FieldMarshal = 0x0D
    | FieldRVA = 0x1D
    | File = 0x26
    | GenericParam = 0x2A
    | GenericParamConstraint = 0x2C
    | ImplMap = 0x1C
    | InterfaceImpl = 0x09
    | ManifestResource = 0x28
    | MemberRef = 0x0A
    | MethodDef = 0x06
    | MethodImpl = 0x19
    | MethodSemantics = 0x18
    | MethodSpec = 0x2B
    | Module = 0x00
    | ModuleRef = 0x1A
    | NestedClass = 0x29
    | Param = 0x08
    | Property = 0x17
    | PropertyMap = 0x15
    | StandAloneSig = 0x11
    | TypeDef = 0x02
    | TypeRef = 0x01
    | TypeSpec = 0x1B
    | FieldPtr = 3
    | MethodPtr = 5
    | ParamPtr = 7
    | EventPtr = 19
    | PropertyPtr = 22
    | EncodingLog = 30
    | EncodingMap = 31

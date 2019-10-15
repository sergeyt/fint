// Contains MDB Table Schemas
module Fint.Schema

open System
open Fint.Enums
open Fint.Types
open Fint.CodedIndex

let I32 = Int32(0)
let I16 = Int16(int16 0)
let STR = StringIndex(0)
let BLOB = BlobIndex(0)
let CodedIdx id = CodedIndex(codedIndexMap.[id])

let Table id =
    TableIndex({ table = id
                 index = 0 })

// 22.2 Assembly : 0x20
module Assembly =
    let HashAlgId : Column =
        { index = 0
          name = "HashAlgId"
          value = I32 }

    let MajorVersion : Column =
        { index = 1
          name = "MajorVersion"
          value = I16 }

    let MinorVersion : Column =
        { index = 2
          name = "MinorVersion"
          value = I16 }

    let BuildNumber : Column =
        { index = 3
          name = "BuildNumber"
          value = I16 }

    let RevisionNumber : Column =
        { index = 4
          name = "RevisionNumber"
          value = I16 }

    // see AssemblyFlags
    let Flags : Column =
        { index = 5
          name = "Flags"
          value = I32 }

    let PublicKey : Column =
        { index = 6
          name = "PublicKey"
          value = BLOB }

    let Name : Column =
        { index = 7
          name = "Name"
          value = STR }

    let Culture : Column =
        { index = 8
          name = "Culture"
          value = STR }

    // Assembly Columns
    let Columns =
        [| HashAlgId; MajorVersion; MinorVersion; BuildNumber; RevisionNumber;
           Flags; PublicKey; Name; Culture |]

// 22.3 AssemblyOS : 0x22
module AssemblyOS =
    let OSPlatformId : Column =
        { index = 0
          name = "OSPlatformId"
          value = I32 }

    let OSMajorVersion : Column =
        { index = 1
          name = "OSMajorVersion"
          value = I32 }

    let OSMinorVersion : Column =
        { index = 2
          name = "OSMinorVersion"
          value = I32 }

    // AssemblyOS Columns
    let Columns = [| OSPlatformId; OSMajorVersion; OSMinorVersion |]

// 22.4 AssemblyProcessor : 0x21
module AssemblyProcessor =
    let Processor : Column =
        { index = 0
          name = "Processor"
          value = I32 }

    // AssemblyProcessor Columns
    let Columns = [| Processor |]

// 22.5 AssemblyRef : 0x23
module AssemblyRef =
    let MajorVersion : Column =
        { index = 0
          name = "MajorVersion"
          value = I16 }

    let MinorVersion : Column =
        { index = 1
          name = "MinorVersion"
          value = I16 }

    let BuildNumber : Column =
        { index = 2
          name = "BuildNumber"
          value = I16 }

    let RevisionNumber : Column =
        { index = 3
          name = "RevisionNumber"
          value = I16 }

    // see AssemblyFlags
    let Flags : Column =
        { index = 4
          name = "Flags"
          value = I32 }

    let PublicKeyOrToken : Column =
        { index = 5
          name = "PublicKeyOrToken"
          value = BLOB }

    let Name : Column =
        { index = 6
          name = "Name"
          value = STR }

    let Culture : Column =
        { index = 7
          name = "Culture"
          value = STR }

    let HashValue : Column =
        { index = 8
          name = "HashValue"
          value = BLOB }

    // AssemblyRef Columns
    let Columns =
        [| MajorVersion; MinorVersion; BuildNumber; RevisionNumber; Flags;
           PublicKeyOrToken; Name; Culture; HashValue |]

// 22.6 AssemblyRefOS : 0x25
module AssemblyRefOS =
    let OSPlatformId : Column =
        { index = 0
          name = "OSPlatformId"
          value = I32 }

    let OSMajorVersion : Column =
        { index = 1
          name = "OSMajorVersion"
          value = I32 }

    let OSMinorVersion : Column =
        { index = 2
          name = "OSMinorVersion"
          value = I32 }

    let AssemblyRef : Column =
        { index = 3
          name = "AssemblyRef"
          value = Table TableId.AssemblyRef }

    // AssemblyRefOS Columns
    let Columns =
        [| OSPlatformId; OSMajorVersion; OSMinorVersion; AssemblyRef |]

// 22.7 AssemblyRefProcessor : 0x24
module AssemblyRefProcessor =
    let Processor : Column =
        { index = 0
          name = "Processor"
          value = I32 }

    let AssemblyRef : Column =
        { index = 1
          name = "AssemblyRef"
          value = Table TableId.AssemblyRef }

    // AssemblyRefProcessor Columns
    let Columns = [| Processor; AssemblyRef |]

// 22.8 The ClassLayout table is used to define how the fields of a class or value type shall be laid out by the CLI.
module ClassLayout =
    let PackingSize : Column =
        { index = 0
          name = "PackingSize"
          value = I16 }

    let ClassSize : Column =
        { index = 1
          name = "ClassSize"
          value = I32 }

    let Parent : Column =
        { index = 2
          name = "Parent"
          value = Table TableId.TypeDef }

    // ClassLayout Columns
    let Columns = [| PackingSize; ClassSize; Parent |]

// 22.9 The Constant table is used to store compile-time, constant values for fields, parameters, and properties.
module Constant =
    let Type : Column =
        { index = 0
          name = "Type"
          value = I16 }

    let Parent : Column =
        { index = 1
          name = "Parent"
          value = CodedIdx CodedIndexId.HasConstant }

    let Value : Column =
        { index = 2
          name = "Value"
          value = BLOB }

    // Constant Columns
    let Columns = [| Type; Parent; Value |]

// 22.10 CustomAttribute : 0x0C
module CustomAttribute =
    let Parent : Column =
        { index = 0
          name = "Parent"
          value = CodedIdx CodedIndexId.HasCustomAttribute }

    let Type : Column =
        { index = 1
          name = "Type"
          value = CodedIdx CodedIndexId.CustomAttributeType }

    let Value : Column =
        { index = 2
          name = "Value"
          value = BLOB }

    // CustomAttribute Columns
    let Columns = [| Parent; Type; Value |]

// 22.11 DeclSecurity : 0x0E
module DeclSecurity =
    let Action : Column =
        { index = 0
          name = "Action"
          value = I16 }

    let Parent : Column =
        { index = 1
          name = "Parent"
          value = CodedIdx CodedIndexId.HasDeclSecurity }

    let PermissionSet : Column =
        { index = 2
          name = "PermissionSet"
          value = BLOB }

    // DeclSecurity Columns
    let Columns = [| Action; Parent; PermissionSet |]

// 22.12 EventMap : 0x12
module EventMap =
    let Parent : Column =
        { index = 0
          name = "Parent"
          value = Table TableId.TypeDef }

    let EventList : Column =
        { index = 1
          name = "EventList"
          value = Table TableId.Event }

    // EventMap Columns
    let Columns = [| Parent; EventList |]

// 22.13 Event : 0x14
module Event =
    // see EventAttributes
    let EventFlags : Column =
        { index = 0
          name = "EventFlags"
          value = I16 }

    let Name : Column =
        { index = 1
          name = "Name"
          value = STR }

    let EventType : Column =
        { index = 2
          name = "EventType"
          value = CodedIdx CodedIndexId.TypeDefOrRef }

    // Event Columns
    let Columns = [| EventFlags; Name; EventType |]

// 22.14 ExportedType : 0x27
module ExportedType =
    // see TypeAttributes
    let Flags : Column =
        { index = 0
          name = "Flags"
          value = I32 }

    let TypeDefId : Column =
        { index = 1
          name = "TypeDefId"
          value = I32 }

    let TypeName : Column =
        { index = 2
          name = "TypeName"
          value = STR }

    let TypeNamespace : Column =
        { index = 3
          name = "TypeNamespace"
          value = STR }

    let Implementation : Column =
        { index = 4
          name = "Implementation"
          value = CodedIdx CodedIndexId.Implementation }

    // ExportedType Columns
    let Columns =
        [| Flags; TypeDefId; TypeName; TypeNamespace; Implementation |]

// 22.15 Field : 0x04
module Field =
    // see FieldAttributes
    let Flags : Column =
        { index = 0
          name = "Flags"
          value = I16 }

    let Name : Column =
        { index = 1
          name = "Name"
          value = STR }

    let Signature : Column =
        { index = 2
          name = "Signature"
          value = BLOB }

    // Field Columns
    let Columns = [| Flags; Name; Signature |]

// 22.16 FieldLayout : 0x10
module FieldLayout =
    let Offset : Column =
        { index = 0
          name = "Offset"
          value = I32 }

    let Field : Column =
        { index = 1
          name = "Field"
          value = Table TableId.Field }

    // FieldLayout Columns
    let Columns = [| Offset; Field |]

// 22.17 FieldMarshal : 0x0D
module FieldMarshal =
    let Parent : Column =
        { index = 0
          name = "Parent"
          value = CodedIdx CodedIndexId.HasFieldMarshal }

    let NativeType : Column =
        { index = 1
          name = "NativeType"
          value = BLOB }

    // FieldMarshal Columns
    let Columns = [| Parent; NativeType |]

// 22.18 FieldRVA : 0x1D
module FieldRVA =
    // 0. RVA : Int32
    let RVA : Column =
        { index = 0
          name = "RVA"
          value = I32 }

    let Field : Column =
        { index = 1
          name = "Field"
          value = Table TableId.Field }

    // FieldRVA Columns
    let Columns = [| RVA; Field |]

// 22.19 File : 0x26
module File =
    // see FileFlags
    let Flags : Column =
        { index = 0
          name = "Flags"
          value = I32 }

    let Name : Column =
        { index = 1
          name = "Name"
          value = STR }

    let HashValue : Column =
        { index = 2
          name = "HashValue"
          value = BLOB }

    // File Columns
    let Columns = [| Flags; Name; HashValue |]

// 22.20 GenericParam : 0x2A
module GenericParam =
    let Number : Column =
        { index = 0
          name = "Number"
          value = I16 }

    // see GenericParamAttributes
    let Flags : Column =
        { index = 1
          name = "Flags"
          value = I16 }

    let Owner : Column =
        { index = 2
          name = "Owner"
          value = CodedIdx CodedIndexId.TypeOrMethodDef }

    let Name : Column =
        { index = 3
          name = "Name"
          value = STR }

    // GenericParam Columns
    let Columns = [| Number; Flags; Owner; Name |]

// 22.21 GenericParamConstraint : 0x2C
module GenericParamConstraint =
    let Owner : Column =
        { index = 0
          name = "Owner"
          value = Table TableId.GenericParam }

    let Constraint : Column =
        { index = 1
          name = "Constraint"
          value = CodedIdx CodedIndexId.TypeDefOrRef }

    // GenericParamConstraint Columns
    let Columns = [| Owner; Constraint |]

// 22.22 ImplMap : 0x1C
module ImplMap =
    // see PInvokeAttributes
    let MappingFlags : Column =
        { index = 0
          name = "MappingFlags"
          value = I16 }

    let MemberForwarded : Column =
        { index = 1
          name = "MemberForwarded"
          value = CodedIdx CodedIndexId.MemberForwarded }

    let ImportName : Column =
        { index = 2
          name = "ImportName"
          value = STR }

    let ImportScope : Column =
        { index = 3
          name = "ImportScope"
          value = Table TableId.ModuleRef }

    // ImplMap Columns
    let Columns = [| MappingFlags; MemberForwarded; ImportName; ImportScope |]

// 22.23 InterfaceImpl : 0x09
module InterfaceImpl =
    let Class : Column =
        { index = 0
          name = "Class"
          value = Table TableId.TypeDef }

    let Interface : Column =
        { index = 1
          name = "Interface"
          value = CodedIdx CodedIndexId.TypeDefOrRef }

    // InterfaceImpl Columns
    let Columns = [| Class; Interface |]

// 22.24 ManifestResource : 0x28
module ManifestResource =
    let Offset : Column =
        { index = 0
          name = "Offset"
          value = I32 }

    // see ManifestResourceAttributes
    let Flags : Column =
        { index = 1
          name = "Flags"
          value = I32 }

    let Name : Column =
        { index = 2
          name = "Name"
          value = STR }

    let Implementation : Column =
        { index = 3
          name = "Implementation"
          value = CodedIdx CodedIndexId.Implementation }

    // ManifestResource Columns
    let Columns = [| Offset; Flags; Name; Implementation |]

// 22.25 The MemberRef table combines two sorts of references, to Methods and to Fields of a class, known as MethodRef and FieldRef, respectively.
module MemberRef =
    let Class : Column =
        { index = 0
          name = "Class"
          value = CodedIdx CodedIndexId.MemberRefParent }

    let Name : Column =
        { index = 1
          name = "Name"
          value = STR }

    let Signature : Column =
        { index = 2
          name = "Signature"
          value = BLOB }

    // MemberRef Columns
    let Columns = [| Class; Name; Signature |]

// 22.26 MethodDef : 0x06
module MethodDef =
    let RVA : Column =
        { index = 0
          name = "RVA"
          value = I32 }

    // see MethodImplAttributes
    let ImplFlags : Column =
        { index = 1
          name = "ImplFlags"
          value = I16 }

    // see MethodAttributes
    let Flags : Column =
        { index = 2
          name = "Flags"
          value = I16 }

    let Name : Column =
        { index = 3
          name = "Name"
          value = STR }

    let Signature : Column =
        { index = 4
          name = "Signature"
          value = BLOB }

    let ParamList : Column =
        { index = 5
          name = "ParamList"
          value = Table TableId.Param }

    // MethodDef Columns
    let Columns = [| RVA; ImplFlags; Flags; Name; Signature; ParamList |]

// 22.27 MethodImpl tables let a compiler override the default inheritance rules provided by the CLI.
module MethodImpl =
    let Class : Column =
        { index = 0
          name = "Class"
          value = Table TableId.TypeDef }

    let MethodBody : Column =
        { index = 1
          name = "MethodBody"
          value = CodedIdx CodedIndexId.MethodDefOrRef }

    let MethodDeclaration : Column =
        { index = 2
          name = "MethodDeclaration"
          value = CodedIdx CodedIndexId.MethodDefOrRef }

    // MethodImpl Columns
    let Columns = [| Class; MethodBody; MethodDeclaration |]

// 22.28 MethodSemantics : 0x18
module MethodSemantics =
    // see MethodSemanticsAttributes
    let Semantics : Column =
        { index = 0
          name = "Semantics"
          value = I16 }

    let Method : Column =
        { index = 1
          name = "Method"
          value = Table TableId.MethodDef }

    let Association : Column =
        { index = 2
          name = "Association"
          value = CodedIdx CodedIndexId.HasSemantics }

    // MethodSemantics Columns
    let Columns = [| Semantics; Method; Association |]

// 22.29 MethodSpec : 0x2B
module MethodSpec =
    let Method : Column =
        { index = 0
          name = "Method"
          value = CodedIdx CodedIndexId.MethodDefOrRef }

    let Instantiation : Column =
        { index = 1
          name = "Instantiation"
          value = BLOB }

    // MethodSpec Columns
    let Columns = [| Method; Instantiation |]

// 22.30 Module : 0x00
module Module =
    let Generation : Column =
        { index = 0
          name = "Generation"
          value = I16 }

    let Name : Column =
        { index = 1
          name = "Name"
          value = STR }

    let Mvid : Column =
        { index = 2
          name = "Mvid"
          value = GuidIndex(0) }

    let EncId : Column =
        { index = 3
          name = "EncId"
          value = GuidIndex(0) }

    let EncBaseId : Column =
        { index = 4
          name = "EncBaseId"
          value = GuidIndex(0) }

    // Module Columns
    let Columns = [| Generation; Name; Mvid; EncId; EncBaseId |]

// 22.31 ModuleRef : 0x1A
module ModuleRef =
    let Name : Column =
        { index = 0
          name = "Name"
          value = STR }

    // ModuleRef Columns
    let Columns = [| Name |]

// 22.32 NestedClass : 0x29
module NestedClass =
    let Class : Column =
        { index = 0
          name = "Class"
          value = Table TableId.TypeDef }

    let EnclosingClass : Column =
        { index = 1
          name = "EnclosingClass"
          value = Table TableId.TypeDef }

    // NestedClass Columns
    let Columns = [| Class; EnclosingClass |]

// 22.33 Param : 0x08
module Param =
    // see ParamAttributes
    let Flags : Column =
        { index = 0
          name = "Flags"
          value = I16 }

    let Sequence : Column =
        { index = 1
          name = "Sequence"
          value = I16 }

    let Name : Column =
        { index = 2
          name = "Name"
          value = STR }

    // Param Columns
    let Columns = [| Flags; Sequence; Name |]

// 22.34 Property : 0x17
module Property =
    // see PropertyAttributes
    let Flags : Column =
        { index = 0
          name = "Flags"
          value = I16 }

    let Name : Column =
        { index = 1
          name = "Name"
          value = STR }

    let Type : Column =
        { index = 2
          name = "Type"
          value = BLOB }

    // Property Columns
    let Columns = [| Flags; Name; Type |]

// 22.35 PropertyMap : 0x15
module PropertyMap =
    let Parent : Column =
        { index = 0
          name = "Parent"
          value = Table TableId.TypeDef }

    let PropertyList : Column =
        { index = 1
          name = "PropertyList"
          value = Table TableId.Property }

    // PropertyMap Columns
    let Columns = [| Parent; PropertyList |]

// 22.36 StandAloneSig : 0x11
module StandAloneSig =
    let Signature : Column =
        { index = 0
          name = "Signature"
          value = BLOB }

    // StandAloneSig Columns
    let Columns = [| Signature |]

// 22.37 TypeDef : 0x02
module TypeDef =
    // see TypeAttributes
    let Flags : Column =
        { index = 0
          name = "Flags"
          value = I32 }

    let TypeName : Column =
        { index = 1
          name = "TypeName"
          value = STR }

    let TypeNamespace : Column =
        { index = 2
          name = "TypeNamespace"
          value = STR }

    let Extends : Column =
        { index = 3
          name = "Extends"
          value = CodedIdx CodedIndexId.TypeDefOrRef }

    let FieldList : Column =
        { index = 4
          name = "FieldList"
          value = Table TableId.Field }

    let MethodList : Column =
        { index = 5
          name = "MethodList"
          value = Table TableId.MethodDef }

    // TypeDef Columns
    let Columns =
        [| Flags; TypeName; TypeNamespace; Extends; FieldList; MethodList |]

// 22.38 TypeRef : 0x01
module TypeRef =
    let ResolutionScope : Column =
        { index = 0
          name = "ResolutionScope"
          value = CodedIdx CodedIndexId.ResolutionScope }

    let TypeName : Column =
        { index = 1
          name = "TypeName"
          value = STR }

    let TypeNamespace : Column =
        { index = 2
          name = "TypeNamespace"
          value = STR }

    // TypeRef Columns
    let Columns = [| ResolutionScope; TypeName; TypeNamespace |]

// 22.39 TypeSpec : 0x1B
module TypeSpec =
    let Signature : Column =
        { index = 0
          name = "Signature"
          value = BLOB }

    // TypeSpec Columns
    let Columns = [| Signature |]

// FieldPtr : 3
module FieldPtr =
    let Field : Column =
        { index = 0
          name = "Field"
          value = Table TableId.Field }

    // FieldPtr Columns
    let Columns = [| Field |]

// MethodPtr : 5
module MethodPtr =
    let Method : Column =
        { index = 0
          name = "Method"
          value = Table TableId.MethodDef }

    // MethodPtr Columns
    let Columns = [| Method |]

// ParamPtr : 7
module ParamPtr =
    let Param : Column =
        { index = 0
          name = "Param"
          value = Table TableId.Param }

    // ParamPtr Columns
    let Columns = [| Param |]

// EventPtr : 19
module EventPtr =
    let Event : Column =
        { index = 0
          name = "Event"
          value = Table TableId.Event }

    // EventPtr Columns
    let Columns = [| Event |]

// PropertyPtr : 22
module PropertyPtr =
    let Property : Column =
        { index = 0
          name = "Property"
          value = Table TableId.Property }

    // PropertyPtr Columns
    let Columns = [| Property |]

// EncodingLog : 30
module EncodingLog =
    let Token : Column =
        { index = 0
          name = "Token"
          value = I32 }

    let FuncCode : Column =
        { index = 1
          name = "FuncCode"
          value = I32 }

    // EncodingLog Columns
    let Columns = [| Token; FuncCode |]

// EncodingMap : 31
module EncodingMap =
    let Token : Column =
        { index = 0
          name = "Token"
          value = I32 }

    // EncodingMap Columns
    let Columns = [| Token |]

let TableColumns id =
    match id with
    | TableId.Assembly -> Assembly.Columns
    | TableId.AssemblyOS -> AssemblyOS.Columns
    | TableId.AssemblyProcessor -> AssemblyProcessor.Columns
    | TableId.AssemblyRef -> AssemblyRef.Columns
    | TableId.AssemblyRefOS -> AssemblyRefOS.Columns
    | TableId.AssemblyRefProcessor -> AssemblyRefProcessor.Columns
    | TableId.ClassLayout -> ClassLayout.Columns
    | TableId.Constant -> Constant.Columns
    | TableId.CustomAttribute -> CustomAttribute.Columns
    | TableId.DeclSecurity -> DeclSecurity.Columns
    | TableId.EventMap -> EventMap.Columns
    | TableId.Event -> Event.Columns
    | TableId.ExportedType -> ExportedType.Columns
    | TableId.Field -> Field.Columns
    | TableId.FieldLayout -> FieldLayout.Columns
    | TableId.FieldMarshal -> FieldMarshal.Columns
    | TableId.FieldRVA -> FieldRVA.Columns
    | TableId.File -> File.Columns
    | TableId.GenericParam -> GenericParam.Columns
    | TableId.GenericParamConstraint -> GenericParamConstraint.Columns
    | TableId.ImplMap -> ImplMap.Columns
    | TableId.InterfaceImpl -> InterfaceImpl.Columns
    | TableId.ManifestResource -> ManifestResource.Columns
    | TableId.MemberRef -> MemberRef.Columns
    | TableId.MethodDef -> MethodDef.Columns
    | TableId.MethodImpl -> MethodImpl.Columns
    | TableId.MethodSemantics -> MethodSemantics.Columns
    | TableId.MethodSpec -> MethodSpec.Columns
    | TableId.Module -> Module.Columns
    | TableId.ModuleRef -> ModuleRef.Columns
    | TableId.NestedClass -> NestedClass.Columns
    | TableId.Param -> Param.Columns
    | TableId.Property -> Property.Columns
    | TableId.PropertyMap -> PropertyMap.Columns
    | TableId.StandAloneSig -> StandAloneSig.Columns
    | TableId.TypeDef -> TypeDef.Columns
    | TableId.TypeRef -> TypeRef.Columns
    | TableId.TypeSpec -> TypeSpec.Columns
    | TableId.FieldPtr -> FieldPtr.Columns
    | TableId.MethodPtr -> MethodPtr.Columns
    | TableId.ParamPtr -> ParamPtr.Columns
    | TableId.EventPtr -> EventPtr.Columns
    | TableId.PropertyPtr -> PropertyPtr.Columns
    | TableId.EncodingLog -> EncodingLog.Columns
    | TableId.EncodingMap -> EncodingMap.Columns
    | _ -> raise <| ArgumentOutOfRangeException("id")

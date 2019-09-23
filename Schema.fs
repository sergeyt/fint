// Contains MDB Table Schemas
module Fint.Schema

open System
open Fint.Enums
open Fint.Meta

let CodedIdx id = Some(codedIndexMap.[id])

// 22.2 Assembly : 0x20
module Assembly =
    let HashAlgId : Column =
        { name = "HashAlgId"
          valueType = Int32
          table = None
          codedIndex = None }
    let MajorVersion : Column =
        { name = "MajorVersion"
          valueType = Int16
          table = None
          codedIndex = None }
    let MinorVersion : Column =
        { name = "MinorVersion"
          valueType = Int16
          table = None
          codedIndex = None }
    let BuildNumber : Column =
        { name = "BuildNumber"
          valueType = Int16
          table = None
          codedIndex = None }
    let RevisionNumber : Column =
        { name = "RevisionNumber"
          valueType = Int16
          table = None
          codedIndex = None }
    // see AssemblyFlags
    let Flags : Column =
        { name = "Flags"
          valueType = Int32
          table = None
          codedIndex = None }
    let PublicKey : Column =
        { name = "PublicKey"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Culture : Column =
        { name = "Culture"
          valueType = StringIndex
          table = None
          codedIndex = None }
    // Assembly Columns
    let Columns =
        [| HashAlgId; MajorVersion; MinorVersion; BuildNumber; RevisionNumber;
           Flags; PublicKey; Name; Culture |]

// 22.3 AssemblyOS : 0x22
module AssemblyOS =
    let OSPlatformId : Column =
        { name = "OSPlatformId"
          valueType = Int32
          table = None
          codedIndex = None }
    let OSMajorVersion : Column =
        { name = "OSMajorVersion"
          valueType = Int32
          table = None
          codedIndex = None }
    let OSMinorVersion : Column =
        { name = "OSMinorVersion"
          valueType = Int32
          table = None
          codedIndex = None }
    // AssemblyOS Columns
    let Columns = [| OSPlatformId; OSMajorVersion; OSMinorVersion |]

// 22.4 AssemblyProcessor : 0x21
module AssemblyProcessor =
    let Processor : Column =
        { name = "Processor"
          valueType = Int32
          table = None
          codedIndex = None }
    // AssemblyProcessor Columns
    let Columns = [| Processor |]

// 22.5 AssemblyRef : 0x23
module AssemblyRef =
    let MajorVersion : Column =
        { name = "MajorVersion"
          valueType = Int16
          table = None
          codedIndex = None }
    let MinorVersion : Column =
        { name = "MinorVersion"
          valueType = Int16
          table = None
          codedIndex = None }
    let BuildNumber : Column =
        { name = "BuildNumber"
          valueType = Int16
          table = None
          codedIndex = None }
    let RevisionNumber : Column =
        { name = "RevisionNumber"
          valueType = Int16
          table = None
          codedIndex = None }
    // see AssemblyFlags
    let Flags : Column =
        { name = "Flags"
          valueType = Int32
          table = None
          codedIndex = None }
    let PublicKeyOrToken : Column =
        { name = "PublicKeyOrToken"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Culture : Column =
        { name = "Culture"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let HashValue : Column =
        { name = "HashValue"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // AssemblyRef Columns
    let Columns =
        [| MajorVersion; MinorVersion; BuildNumber; RevisionNumber; Flags;
           PublicKeyOrToken; Name; Culture; HashValue |]

// 22.6 AssemblyRefOS : 0x25
module AssemblyRefOS =
    let OSPlatformId : Column =
        { name = "OSPlatformId"
          valueType = Int32
          table = None
          codedIndex = None }
    let OSMajorVersion : Column =
        { name = "OSMajorVersion"
          valueType = Int32
          table = None
          codedIndex = None }
    let OSMinorVersion : Column =
        { name = "OSMinorVersion"
          valueType = Int32
          table = None
          codedIndex = None }
    let AssemblyRef : Column =
        { name = "AssemblyRef"
          valueType = TableIndex
          table = Some TableId.AssemblyRef
          codedIndex = None }
    // AssemblyRefOS Columns
    let Columns =
        [| OSPlatformId; OSMajorVersion; OSMinorVersion; AssemblyRef |]

// 22.7 AssemblyRefProcessor : 0x24
module AssemblyRefProcessor =
    let Processor : Column =
        { name = "Processor"
          valueType = Int32
          table = None
          codedIndex = None }
    let AssemblyRef : Column =
        { name = "AssemblyRef"
          valueType = TableIndex
          table = Some TableId.AssemblyRef
          codedIndex = None }
    // AssemblyRefProcessor Columns
    let Columns = [| Processor; AssemblyRef |]

// 22.8 The ClassLayout table is used to define how the fields of a class or value type shall be laid out by the CLI.
module ClassLayout =
    let PackingSize : Column =
        { name = "PackingSize"
          valueType = Int16
          table = None
          codedIndex = None }
    let ClassSize : Column =
        { name = "ClassSize"
          valueType = Int32
          table = None
          codedIndex = None }
    let Parent : Column =
        { name = "Parent"
          valueType = TableIndex
          table = Some TableId.TypeDef
          codedIndex = None }
    // ClassLayout Columns
    let Columns = [| PackingSize; ClassSize; Parent |]

// 22.9 The Constant table is used to store compile-time, constant values for fields, parameters, and properties.
module Constant =
    let Type : Column =
        { name = "Type"
          valueType = Int16
          table = None
          codedIndex = None }
    let Parent : Column =
        { name = "Parent"
          valueType = CodedIndex
          codedIndex = CodedIdx HasConstant
          table = None }
    let Value : Column =
        { name = "Value"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // Constant Columns
    let Columns = [| Type; Parent; Value |]

// 22.10 CustomAttribute : 0x0C
module CustomAttribute =
    let Parent : Column =
        { name = "Parent"
          valueType = CodedIndex
          codedIndex = CodedIdx HasCustomAttribute
          table = None }
    let Type : Column =
        { name = "Type"
          valueType = CodedIndex
          codedIndex = CodedIdx CustomAttributeType
          table = None }
    let Value : Column =
        { name = "Value"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // CustomAttribute Columns
    let Columns = [| Parent; Type; Value |]

// 22.11 DeclSecurity : 0x0E
module DeclSecurity =
    let Action : Column =
        { name = "Action"
          valueType = Int16
          table = None
          codedIndex = None }
    let Parent : Column =
        { name = "Parent"
          valueType = CodedIndex
          codedIndex = CodedIdx HasDeclSecurity
          table = None }
    let PermissionSet : Column =
        { name = "PermissionSet"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // DeclSecurity Columns
    let Columns = [| Action; Parent; PermissionSet |]

// 22.12 EventMap : 0x12
module EventMap =
    let Parent : Column =
        { name = "Parent"
          valueType = TableIndex
          table = Some TableId.TypeDef
          codedIndex = None }
    let EventList : Column =
        { name = "EventList"
          valueType = TableIndex
          table = Some TableId.Event
          codedIndex = None }
    // EventMap Columns
    let Columns = [| Parent; EventList |]

// 22.13 Event : 0x14
module Event =
    // see EventAttributes
    let EventFlags : Column =
        { name = "EventFlags"
          valueType = Int16
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let EventType : Column =
        { name = "EventType"
          valueType = CodedIndex
          codedIndex = CodedIdx TypeDefOrRef
          table = None }
    // Event Columns
    let Columns = [| EventFlags; Name; EventType |]

// 22.14 ExportedType : 0x27
module ExportedType =
    // see TypeAttributes
    let Flags : Column =
        { name = "Flags"
          valueType = Int32
          table = None
          codedIndex = None }
    let TypeDefId : Column =
        { name = "TypeDefId"
          valueType = Int32
          table = None
          codedIndex = None }
    let TypeName : Column =
        { name = "TypeName"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let TypeNamespace : Column =
        { name = "TypeNamespace"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Implementation : Column =
        { name = "Implementation"
          valueType = CodedIndex
          codedIndex = CodedIdx Implementation
          table = None }
    // ExportedType Columns
    let Columns =
        [| Flags; TypeDefId; TypeName; TypeNamespace; Implementation |]

// 22.15 Field : 0x04
module Field =
    // see FieldAttributes
    let Flags : Column =
        { name = "Flags"
          valueType = Int16
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Signature : Column =
        { name = "Signature"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // Field Columns
    let Columns = [| Flags; Name; Signature |]

// 22.16 FieldLayout : 0x10
module FieldLayout =
    let Offset : Column =
        { name = "Offset"
          valueType = Int32
          table = None
          codedIndex = None }
    let Field : Column =
        { name = "Field"
          valueType = TableIndex
          table = Some TableId.Field
          codedIndex = None }
    // FieldLayout Columns
    let Columns = [| Offset; Field |]

// 22.17 FieldMarshal : 0x0D
module FieldMarshal =
    let Parent : Column =
        { name = "Parent"
          valueType = CodedIndex
          codedIndex = CodedIdx HasFieldMarshal
          table = None }
    let NativeType : Column =
        { name = "NativeType"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // FieldMarshal Columns
    let Columns = [| Parent; NativeType |]

// 22.18 FieldRVA : 0x1D
module FieldRVA =
    // 0. RVA : Int32
    let RVA : Column =
        { name = "RVA"
          valueType = Int32
          table = None
          codedIndex = None }
    let Field : Column =
        { name = "Field"
          valueType = TableIndex
          table = Some TableId.Field
          codedIndex = None }
    // FieldRVA Columns
    let Columns = [| RVA; Field |]

// 22.19 File : 0x26
module File =
    // see FileFlags
    let Flags : Column =
        { name = "Flags"
          valueType = Int32
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let HashValue : Column =
        { name = "HashValue"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // File Columns
    let Columns = [| Flags; Name; HashValue |]

// 22.20 GenericParam : 0x2A
module GenericParam =
    let Number : Column =
        { name = "Number"
          valueType = Int16
          table = None
          codedIndex = None }
    // see GenericParamAttributes
    let Flags : Column =
        { name = "Flags"
          valueType = Int16
          table = None
          codedIndex = None }
    let Owner : Column =
        { name = "Owner"
          valueType = CodedIndex
          codedIndex = CodedIdx TypeOrMethodDef
          table = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    // GenericParam Columns
    let Columns = [| Number; Flags; Owner; Name |]

// 22.21 GenericParamConstraint : 0x2C
module GenericParamConstraint =
    let Owner : Column =
        { name = "Owner"
          valueType = TableIndex
          table = Some TableId.GenericParam
          codedIndex = None }
    let Constraint : Column =
        { name = "Constraint"
          valueType = CodedIndex
          codedIndex = CodedIdx TypeDefOrRef
          table = None }
    // GenericParamConstraint Columns
    let Columns = [| Owner; Constraint |]

// 22.22 ImplMap : 0x1C
module ImplMap =
    // see PInvokeAttributes
    let MappingFlags : Column =
        { name = "MappingFlags"
          valueType = Int16
          table = None
          codedIndex = None }
    let MemberForwarded : Column =
        { name = "MemberForwarded"
          valueType = CodedIndex
          codedIndex = CodedIdx MemberForwarded
          table = None }
    let ImportName : Column =
        { name = "ImportName"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let ImportScope : Column =
        { name = "ImportScope"
          valueType = TableIndex
          table = Some TableId.ModuleRef
          codedIndex = None }
    // ImplMap Columns
    let Columns = [| MappingFlags; MemberForwarded; ImportName; ImportScope |]

// 22.23 InterfaceImpl : 0x09
module InterfaceImpl =
    let Class : Column =
        { name = "Class"
          valueType = TableIndex
          table = Some TableId.TypeDef
          codedIndex = None }
    let Interface : Column =
        { name = "Interface"
          valueType = CodedIndex
          codedIndex = CodedIdx TypeDefOrRef
          table = None }
    // InterfaceImpl Columns
    let Columns = [| Class; Interface |]

// 22.24 ManifestResource : 0x28
module ManifestResource =
    let Offset : Column =
        { name = "Offset"
          valueType = Int32
          table = None
          codedIndex = None }
    // see ManifestResourceAttributes
    let Flags : Column =
        { name = "Flags"
          valueType = Int32
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Implementation : Column =
        { name = "Implementation"
          valueType = CodedIndex
          codedIndex = CodedIdx Implementation
          table = None }
    // ManifestResource Columns
    let Columns = [| Offset; Flags; Name; Implementation |]

// 22.25 The MemberRef table combines two sorts of references, to Methods and to Fields of a class, known as MethodRef and FieldRef, respectively.
module MemberRef =
    let Class : Column =
        { name = "Class"
          valueType = CodedIndex
          codedIndex = CodedIdx MemberRefParent
          table = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Signature : Column =
        { name = "Signature"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // MemberRef Columns
    let Columns = [| Class; Name; Signature |]

// 22.26 MethodDef : 0x06
module MethodDef =
    let RVA : Column =
        { name = "RVA"
          valueType = Int32
          table = None
          codedIndex = None }
    // see MethodImplAttributes
    let ImplFlags : Column =
        { name = "ImplFlags"
          valueType = Int16
          table = None
          codedIndex = None }
    // see MethodAttributes
    let Flags : Column =
        { name = "Flags"
          valueType = Int16
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Signature : Column =
        { name = "Signature"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    let ParamList : Column =
        { name = "ParamList"
          valueType = TableIndex
          table = Some TableId.Param
          codedIndex = None }
    // MethodDef Columns
    let Columns = [| RVA; ImplFlags; Flags; Name; Signature; ParamList |]

// 22.27 MethodImpl tables let a compiler override the default inheritance rules provided by the CLI.
module MethodImpl =
    let Class : Column =
        { name = "Class"
          valueType = TableIndex
          table = Some TableId.TypeDef
          codedIndex = None }
    let MethodBody : Column =
        { name = "MethodBody"
          valueType = CodedIndex
          codedIndex = CodedIdx MethodDefOrRef
          table = None }
    let MethodDeclaration : Column =
        { name = "MethodDeclaration"
          valueType = CodedIndex
          codedIndex = CodedIdx MethodDefOrRef
          table = None }
    // MethodImpl Columns
    let Columns = [| Class; MethodBody; MethodDeclaration |]

// 22.28 MethodSemantics : 0x18
module MethodSemantics =
    // see MethodSemanticsAttributes
    let Semantics : Column =
        { name = "Semantics"
          valueType = Int16
          table = None
          codedIndex = None }
    let Method : Column =
        { name = "Method"
          valueType = TableIndex
          table = Some TableId.MethodDef
          codedIndex = None }
    let Association : Column =
        { name = "Association"
          valueType = CodedIndex
          codedIndex = CodedIdx HasSemantics
          table = None }
    // MethodSemantics Columns
    let Columns = [| Semantics; Method; Association |]

// 22.29 MethodSpec : 0x2B
module MethodSpec =
    let Method : Column =
        { name = "Method"
          valueType = CodedIndex
          codedIndex = CodedIdx MethodDefOrRef
          table = None }
    let Instantiation : Column =
        { name = "Instantiation"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // MethodSpec Columns
    let Columns = [| Method; Instantiation |]

// 22.30 Module : 0x00
module Module =
    let Generation : Column =
        { name = "Generation"
          valueType = Int16
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Mvid : Column =
        { name = "Mvid"
          valueType = GuidIndex
          table = None
          codedIndex = None }
    let EncId : Column =
        { name = "EncId"
          valueType = GuidIndex
          table = None
          codedIndex = None }
    let EncBaseId : Column =
        { name = "EncBaseId"
          valueType = GuidIndex
          table = None
          codedIndex = None }
    // Module Columns
    let Columns = [| Generation; Name; Mvid; EncId; EncBaseId |]

// 22.31 ModuleRef : 0x1A
module ModuleRef =
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    // ModuleRef Columns
    let Columns = [| Name |]

// 22.32 NestedClass : 0x29
module NestedClass =
    let Class : Column =
        { name = "Class"
          valueType = TableIndex
          table = Some TableId.TypeDef
          codedIndex = None }
    let EnclosingClass : Column =
        { name = "EnclosingClass"
          valueType = TableIndex
          table = Some TableId.TypeDef
          codedIndex = None }
    // NestedClass Columns
    let Columns = [| Class; EnclosingClass |]

// 22.33 Param : 0x08
module Param =
    // see ParamAttributes
    let Flags : Column =
        { name = "Flags"
          valueType = Int16
          table = None
          codedIndex = None }
    let Sequence : Column =
        { name = "Sequence"
          valueType = Int16
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    // Param Columns
    let Columns = [| Flags; Sequence; Name |]

// 22.34 Property : 0x17
module Property =
    // see PropertyAttributes
    let Flags : Column =
        { name = "Flags"
          valueType = Int16
          table = None
          codedIndex = None }
    let Name : Column =
        { name = "Name"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Type : Column =
        { name = "Type"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // Property Columns
    let Columns = [| Flags; Name; Type |]

// 22.35 PropertyMap : 0x15
module PropertyMap =
    let Parent : Column =
        { name = "Parent"
          valueType = TableIndex
          table = Some TableId.TypeDef
          codedIndex = None }
    let PropertyList : Column =
        { name = "PropertyList"
          valueType = TableIndex
          table = Some TableId.Property
          codedIndex = None }
    // PropertyMap Columns
    let Columns = [| Parent; PropertyList |]

// 22.36 StandAloneSig : 0x11
module StandAloneSig =
    let Signature : Column =
        { name = "Signature"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // StandAloneSig Columns
    let Columns = [| Signature |]

// 22.37 TypeDef : 0x02
module TypeDef =
    // see TypeAttributes
    let Flags : Column =
        { name = "Flags"
          valueType = Int32
          table = None
          codedIndex = None }
    let TypeName : Column =
        { name = "TypeName"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let TypeNamespace : Column =
        { name = "TypeNamespace"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let Extends : Column =
        { name = "Extends"
          valueType = CodedIndex
          codedIndex = CodedIdx TypeDefOrRef
          table = None }
    let FieldList : Column =
        { name = "FieldList"
          valueType = TableIndex
          table = Some TableId.Field
          codedIndex = None }
    let MethodList : Column =
        { name = "MethodList"
          valueType = TableIndex
          table = Some TableId.MethodDef
          codedIndex = None }
    // TypeDef Columns
    let Columns =
        [| Flags; TypeName; TypeNamespace; Extends; FieldList; MethodList |]

// 22.38 TypeRef : 0x01
module TypeRef =
    let ResolutionScope : Column =
        { name = "ResolutionScope"
          valueType = CodedIndex
          codedIndex = CodedIdx ResolutionScope
          table = None }
    let TypeName : Column =
        { name = "TypeName"
          valueType = StringIndex
          table = None
          codedIndex = None }
    let TypeNamespace : Column =
        { name = "TypeNamespace"
          valueType = StringIndex
          table = None
          codedIndex = None }
    // TypeRef Columns
    let Columns = [| ResolutionScope; TypeName; TypeNamespace |]

// 22.39 TypeSpec : 0x1B
module TypeSpec =
    let Signature : Column =
        { name = "Signature"
          valueType = BlobIndex
          table = None
          codedIndex = None }
    // TypeSpec Columns
    let Columns = [| Signature |]

// FieldPtr : 3
module FieldPtr =
    let Field : Column =
        { name = "Field"
          valueType = TableIndex
          table = Some TableId.Field
          codedIndex = None }
    // FieldPtr Columns
    let Columns = [| Field |]

// MethodPtr : 5
module MethodPtr =
    let Method : Column =
        { name = "Method"
          valueType = TableIndex
          table = Some TableId.MethodDef
          codedIndex = None }
    // MethodPtr Columns
    let Columns = [| Method |]

// ParamPtr : 7
module ParamPtr =
    let Param : Column =
        { name = "Param"
          valueType = TableIndex
          table = Some TableId.Param
          codedIndex = None }
    // ParamPtr Columns
    let Columns = [| Param |]

// EventPtr : 19
module EventPtr =
    let Event : Column =
        { name = "Event"
          valueType = TableIndex
          table = Some TableId.Event
          codedIndex = None }
    // EventPtr Columns
    let Columns = [| Event |]

// PropertyPtr : 22
module PropertyPtr =
    let Property : Column =
        { name = "Property"
          valueType = TableIndex
          table = Some TableId.Property
          codedIndex = None }
    // PropertyPtr Columns
    let Columns = [| Property |]

// EncodingLog : 30
module EncodingLog =
    let Token : Column =
        { name = "Token"
          valueType = Int32
          table = None
          codedIndex = None }
    let FuncCode : Column =
        { name = "FuncCode"
          valueType = Int32
          table = None
          codedIndex = None }
    // EncodingLog Columns
    let Columns = [| Token; FuncCode |]

// EncodingMap : 31
module EncodingMap =
    let Token : Column =
        { name = "Token"
          valueType = Int32
          table = None
          codedIndex = None }
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
    | _ -> raise (ArgumentOutOfRangeException("id"))

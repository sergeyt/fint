module Fint.CodedIndex

open Fint.Enums

type CodedIndexId =
    | CustomAttributeType = 0
    | HasConstant = 1
    | HasCustomAttribute = 2
    | HasDeclSecurity = 3
    | HasFieldMarshal = 4
    | HasSemantics = 5
    | Implementation = 6
    | MemberForwarded = 7
    | MemberRefParent = 8
    | MethodDefOrRef = 9
    | ResolutionScope = 10
    | TypeDefOrRef = 11
    | TypeOrMethodDef = 12

type CodedIndex =
    { id: CodedIndexId
      bits: int
      tables: TableId array }

let customAttributeType: CodedIndex =
    { id = CodedIndexId.CustomAttributeType
      bits = 3
      tables = [| TableId.TypeRef; TableId.TypeRef; TableId.MethodDef; TableId.MemberRef; TableId.TypeDef |] }

let hasConstant: CodedIndex =
    { id = CodedIndexId.HasConstant
      bits = 2
      tables = [| TableId.Field; TableId.Param; TableId.Property |] }

//NOTE FROM SPEC:
//[Note: HasCustomAttributes only has values for tables that are �externally visible�; that is, that correspond to items
//in a user source program. For example, an attribute can be attached to a TypeDef table and a Field table, but not a
//ClassLayout table. As a result, some table types are missing from the enum above.]
let hasCustomAttribute: CodedIndex =
    { id = CodedIndexId.HasCustomAttribute
      bits = 5
      tables =
          [| TableId.MethodDef
             TableId.Field
             TableId.TypeRef
             TableId.TypeDef
             TableId.Param
             TableId.InterfaceImpl
             TableId.MemberRef
             TableId.Module
             TableId.DeclSecurity
             TableId.Property
             TableId.Event
             TableId.StandAloneSig
             TableId.ModuleRef
             TableId.TypeSpec
             TableId.Assembly
             TableId.AssemblyRef
             TableId.File
             TableId.ExportedType
             TableId.ManifestResource
             TableId.GenericParam |] }

let hasDeclSecurity: CodedIndex =
    { id = CodedIndexId.HasDeclSecurity
      bits = 2
      tables = [| TableId.TypeDef; TableId.MethodDef; TableId.Assembly |] }

let hasFieldMarshal: CodedIndex =
    { id = CodedIndexId.HasFieldMarshal
      bits = 1
      tables = [| TableId.Field; TableId.Param |] }

let hasSemantics: CodedIndex =
    { id = CodedIndexId.HasSemantics
      bits = 1
      tables = [| TableId.Event; TableId.Property |] }

let implementation: CodedIndex =
    { id = CodedIndexId.Implementation
      bits = 2
      tables = [| TableId.File; TableId.AssemblyRef; TableId.ExportedType |] }

let memberForwarded: CodedIndex =
    { id = CodedIndexId.MemberForwarded
      bits = 1
      tables = [| TableId.Field; TableId.MethodDef |] }

let memberRefParent: CodedIndex =
    { id = CodedIndexId.MemberRefParent
      bits = 3
      tables = [| TableId.TypeDef; TableId.TypeRef; TableId.ModuleRef; TableId.MethodDef; TableId.TypeSpec |] }

let methodDefOrRef: CodedIndex =
    { id = CodedIndexId.MethodDefOrRef
      bits = 1
      tables = [| TableId.MethodDef; TableId.MemberRef |] }

let resolutionScope: CodedIndex =
    { id = CodedIndexId.ResolutionScope
      bits = 2
      tables = [| TableId.Module; TableId.ModuleRef; TableId.AssemblyRef; TableId.TypeRef |] }

let typeDefOrRef: CodedIndex =
    { id = CodedIndexId.TypeDefOrRef
      bits = 2
      tables = [| TableId.TypeDef; TableId.TypeRef; TableId.TypeSpec |] }

let typeOrMethodDef: CodedIndex =
    { id = CodedIndexId.TypeOrMethodDef
      bits = 1
      tables = [| TableId.TypeDef; TableId.MethodDef |] }

let codedIndexMap =
    dict
        [ (CodedIndexId.CustomAttributeType, customAttributeType)
          (CodedIndexId.HasConstant, hasConstant)
          (CodedIndexId.HasCustomAttribute, hasCustomAttribute)
          (CodedIndexId.HasDeclSecurity, hasDeclSecurity)
          (CodedIndexId.HasFieldMarshal, hasFieldMarshal)
          (CodedIndexId.HasSemantics, hasSemantics)
          (CodedIndexId.Implementation, implementation)
          (CodedIndexId.MemberForwarded, memberForwarded)
          (CodedIndexId.MemberRefParent, memberRefParent)
          (CodedIndexId.MethodDefOrRef, methodDefOrRef)
          (CodedIndexId.ResolutionScope, resolutionScope)
          (CodedIndexId.TypeDefOrRef, typeDefOrRef)
          (CodedIndexId.TypeOrMethodDef, typeOrMethodDef) ]

type TableIndex =
    { table: TableId
      index: int }

let decodeCodedIndex (meta: CodedIndex, value: uint32) =
    let mask = uint32 (0xff >>> (8 - meta.bits))
    let tag = int (value &&& mask)
    if (tag < 0 || tag >= meta.tables.Length) then invalidArg "value" "bad coded index"
    let index = int (value >>> meta.bits)

    let result: TableIndex =
        { table = meta.tables.[tag]
          index = index }
    result

let decodeTableIndex (token: uint32) =
    let id = int ((token >>> 24) &&& uint32 0xff)
    let index = int (token &&& uint32 0xffffff)

    let result: TableIndex =
        { table = enum id
          index = index }
    result

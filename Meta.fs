module Fint.Meta

open System
open Fint.Enums
open Fint.MethodBody

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
    { id: CodedIndexId;
      bits : int;
      tables : TableId array; }

let customAttributeType : CodedIndex =
    { id = CodedIndexId.CustomAttributeType;
      bits = 3
      tables =
          [| TableId.TypeRef; TableId.TypeRef; TableId.MethodDef;
             TableId.MemberRef; TableId.TypeDef |] }

let codedIndexMap =
    dict [ (CodedIndexId.CustomAttributeType, customAttributeType)
           (CodedIndexId.HasConstant,
            { id = CodedIndexId.HasConstant
              bits = 2
              tables = [| TableId.Field; TableId.Param; TableId.Property |] })

           (//NOTE FROM SPEC:
            //[Note: HasCustomAttributes only has values for tables that are �externally visible�; that is, that correspond to items
            //in a user source program. For example, an attribute can be attached to a TypeDef table and a Field table, but not a
            //ClassLayout table. As a result, some table types are missing from the enum above.]
            CodedIndexId.HasCustomAttribute,
            { id = CodedIndexId.HasCustomAttribute
              bits = 5
              tables =
                  [| TableId.MethodDef; TableId.Field; TableId.TypeRef;
                     TableId.TypeDef; TableId.Param; TableId.InterfaceImpl;
                     TableId.MemberRef; TableId.Module; TableId.DeclSecurity;
                     TableId.Property; TableId.Event; TableId.StandAloneSig;
                     TableId.ModuleRef; TableId.TypeSpec; TableId.Assembly;
                     TableId.AssemblyRef; TableId.File; TableId.ExportedType;
                     TableId.ManifestResource; TableId.GenericParam |] })

           (CodedIndexId.HasDeclSecurity,
            { id = CodedIndexId.HasDeclSecurity
              bits = 2
              tables =
                  [| TableId.TypeDef; TableId.MethodDef; TableId.Assembly |] })
           (CodedIndexId.HasFieldMarshal,
            { id = CodedIndexId.HasFieldMarshal
              bits = 1
              tables = [| TableId.Field; TableId.Param |] })
           (CodedIndexId.HasSemantics,
            { id = CodedIndexId.HasSemantics
              bits = 1
              tables = [| TableId.Event; TableId.Property |] })

           (CodedIndexId.Implementation,
            { id = CodedIndexId.Implementation
              bits = 2
              tables =
                  [| TableId.File; TableId.AssemblyRef; TableId.ExportedType |] })
           (CodedIndexId.MemberForwarded,
            { id = CodedIndexId.MemberForwarded
              bits = 1
              tables = [| TableId.Field; TableId.MethodDef |] })

           (CodedIndexId.MemberRefParent,
            { id = CodedIndexId.MemberRefParent
              bits = 3
              tables =
                  [| TableId.TypeDef; TableId.TypeRef; TableId.ModuleRef;
                     TableId.MethodDef; TableId.TypeSpec |] })
           (CodedIndexId.MethodDefOrRef,
            { id = CodedIndexId.MethodDefOrRef
              bits = 1
              tables = [| TableId.MethodDef; TableId.MemberRef |] })

           (CodedIndexId.ResolutionScope,
            { id = CodedIndexId.ResolutionScope
              bits = 2
              tables =
                  [| TableId.Module; TableId.ModuleRef; TableId.AssemblyRef;
                     TableId.TypeRef |] })

           (CodedIndexId.TypeDefOrRef,
            { id = CodedIndexId.TypeDefOrRef
              bits = 2
              tables = [| TableId.TypeDef; TableId.TypeRef; TableId.TypeSpec |] })
           (CodedIndexId.TypeOrMethodDef,
            { id = CodedIndexId.TypeOrMethodDef
              bits = 1
              tables = [| TableId.TypeDef; TableId.MethodDef |] }) ]

type TableIndex =
    { table : TableId
      index : int }

let decodeCodedIndex (meta : CodedIndex, value : uint32) =
    let mask = uint32 (0xff >>> (8 - meta.bits))
    let tag = int (value &&& mask)
    if (tag < 0 || tag >= meta.tables.Length) then
        invalidArg "value" "bad coded index"
    let index = int (value >>> meta.bits)

    let result : TableIndex =
        { table = meta.tables.[tag]
          index = index }
    result

let decodeTableIndex (token : uint32) =
    let id = int ((token >>> 24) &&& uint32 0xff)
    let index = int (token &&& uint32 0xffffff)

    let result : TableIndex =
        { table = enum id
          index = index }
    result

// data types of metadata column
type ColumnType =
  | Int16 of int16
  | Int32 of int32
  | StringIndex of int32
  | BlobIndex of int32
  | GuidIndex of int32
  | TableIndex of TableIndex
  | CodedIndex of CodedIndex

type Cell =
  | Int16Cell of int16
  | Int32Cell of int32
  | StringCell of (unit -> string)
  | BlobCell of (unit -> byte array)
  | GuidCell of (unit -> Guid)
  | TableIndexCell of TableIndex

type Column = {
  index: int;
  name: string;
  value: ColumnType;
}

type ComputedColumn = {
  name: string;
  value: ColumnType;
  size: int;
}

type Table = {
  id: TableId;
  rowCount: int;
  rowSize: int;
  offset: int64;
  size: int;
  isSorted: bool;
  columns: ComputedColumn array;
}

type Row = {
  table: TableId;
  cells: Cell array;
}

type TokenValue =
  | StringToken of string
  | RowToken of Row

type MethodDef = {
  rva: uint32;
  name: string;
  body: unit -> MethodBody option;
};

type TypeDef = {
  ns: string;
  name: string;
};

type TypeRef = {
  ns: string;
  name: string;
};

type MemberRefParent =
  | TypeDefParent of TypeDef
  | TypeRefParent of TypeRef

type MemberRef = {
  parent: MemberRefParent;
  name: string;
};

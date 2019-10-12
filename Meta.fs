module Fint.Meta

open System
open Fint.Enums
open Fint.MethodBody
open Fint.CodedIndex
open Fint.Signature

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
  flags: MethodAttributes;
  signature: MethodSignature;
  body: unit -> MethodBody option;
  localVars: unit -> LocalVar array;
};

let IsStaticMethod m = int (m.flags &&& MethodAttributes.Static) <> 0
let IsVoidMethod m =
  match m.signature.ReturnType with
  | PrimitiveTypeSig t -> t = ElementType.Void
  | _ -> false

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
  signature: MethodSignature;
};

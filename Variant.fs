module Fint.Variant

open System
open Fint.Utils

type VariantValue =
 | VarNull
 | VarBool of bool
 | VarChar of char
 | VarInt8 of sbyte
 | VarUInt8 of byte
 | VarInt16 of int16
 | VarUInt16 of uint16
 | VarInt32 of int32
 | VarUInt32 of uint32
 | VarInt64 of int64
 | VarUInt64 of uint64
 | VarSingle of float32
 | VarDouble of double
 | VarDecimal of decimal
 | VarString of string
 | VarObject of obj

let roundDouble (v: double) =
    let t = Math.Truncate(v)
    let a = if v - t > 0.5 then 1.0 else 0.0
    t + a
let roundDecimal (v: decimal) =
    let t = Math.Truncate(v)
    let a = if v - t > decimal 0.5 then 1.0 else 0.0
    t + decimal a

let boolI32(v: bool) = if v then 1 else 0
let boolU32(v: bool) = if v then 1u else 0u
let boolI64(v: bool) = if v then 1L else 0L
let boolU64(v: bool) = if v then 1UL else 0UL
let boolSingle(v: bool) = if v then float32 1 else float32 0
let boolDouble(v: bool) = if v then 1.0 else 0.0
let boolDecimal(v: bool) = if v then decimal 1 else decimal 0

let cmpI32(v: int32) =
    if v = 0 then 0
    elif v < 0 then -1
    else 1
let cmpU32(v: uint32) =
    if v = 0u then 0
    elif v < 0u then -1
    else 1
let cmpI64(v: int64) =
    if v = 0L then 0
    elif v < 0L then -1
    else 1
let cmpU64(v: uint64) =
    if v = 0UL then 0
    elif v < 0UL then -1
    else 1
let cmpSingle (v: float32) =
    if v = float32 0 then 0
    elif v < float32 0 then -1
    else 1
let cmpDouble (v: double) =
    if v = 0.0 then 0
    elif v < 0.0 then -1
    else 1
let cmpDecimal (v: decimal) =
    if v = decimal 0 then 0
    elif v < decimal 0 then -1
    else 1

type Variant(value: VariantValue) =
    member val value = value
    static member Of(v: obj) =
        match v with
        | null -> Variant(VarNull)
        | _ ->
            match Type.GetTypeCode(v.GetType()) with
            | TypeCode.Boolean -> Variant(VarBool(v :?> bool))
            | TypeCode.Char -> Variant(VarChar(v :?> char))
            | TypeCode.SByte -> Variant(VarInt8(v :?> sbyte))
            | TypeCode.Byte -> Variant(VarUInt8(v :?> byte))
            | TypeCode.Int16 -> Variant(VarInt16(v :?> int16))
            | TypeCode.UInt16 -> Variant(VarUInt16(v :?> uint16))
            | TypeCode.Int32 -> Variant(VarInt32(v :?> int32))
            | TypeCode.UInt32 -> Variant(VarUInt32(v :?> uint32))
            | TypeCode.Int64 -> Variant(VarInt64(v :?> int64))
            | TypeCode.UInt64 -> Variant(VarUInt64(v :?> uint64))
            | TypeCode.Single -> Variant(VarSingle(v :?> float32))
            | TypeCode.Double -> Variant(VarDouble(v :?> double))
            | TypeCode.Decimal -> Variant(VarDecimal(v :?> decimal))
            | TypeCode.String -> Variant(VarString(v :?> string))
            | _ -> Variant(VarObject(v))
    member this.IsNull() =
        match this.value with
        | VarNull -> true
        | _ -> false
    member this.ToBoolean() =
        match this.value with
        | VarNull -> false
        | VarBool t -> t
        | VarChar t -> int t <> 0
        | VarInt8 t ->  int t <> 0
        | VarUInt8 t -> int t <> 0
        | VarInt16 t -> int t <> 0
        | VarUInt16 t -> int t <> 0
        | VarInt32 t -> t <> 0
        | VarUInt32 t -> t <> 0u
        | VarInt64 t -> t <> 0L
        | VarUInt64 t -> t <> 0UL
        | VarSingle t -> t <> float32 0
        | VarDouble t -> t <> 0.0
        | VarDecimal t -> t <> decimal 0
        | VarString _ -> true
        | VarObject _ -> true
    member this.ToChar() =
        match this.value with
        | VarNull -> char 0
        | VarBool t -> char(boolU32(t))
        | VarChar t -> char t
        | VarInt8 t ->  char t
        | VarUInt8 t -> char t
        | VarInt16 t -> char t
        | VarUInt16 t -> char t
        | VarInt32 t -> char t
        | VarUInt32 t -> char t
        | VarInt64 t -> char t
        | VarUInt64 t -> char t
        | VarSingle t -> char t
        | VarDouble t -> char t
        | VarDecimal t -> char t
        | _ -> notSupported()
    member this.ToSByte() =
        match this.value with
        | VarNull -> sbyte 0
        | VarBool t -> sbyte(boolI32(t))
        | VarChar t -> sbyte t
        | VarInt8 t ->  sbyte t
        | VarUInt8 t -> sbyte t
        | VarInt16 t -> sbyte t
        | VarUInt16 t -> sbyte t
        | VarInt32 t -> sbyte t
        | VarUInt32 t -> sbyte t
        | VarInt64 t -> sbyte t
        | VarUInt64 t -> sbyte t
        | VarSingle t -> sbyte t
        | VarDouble t -> sbyte t
        | VarDecimal t -> sbyte t
        | _ -> notSupported()
    member this.ToByte() =
        match this.value with
        | VarNull -> byte 0
        | VarBool t -> byte(boolI32(t))
        | VarChar t -> byte t
        | VarInt8 t ->  byte t
        | VarUInt8 t -> byte t
        | VarInt16 t -> byte t
        | VarUInt16 t -> byte t
        | VarInt32 t -> byte t
        | VarUInt32 t -> byte t
        | VarInt64 t -> byte t
        | VarUInt64 t -> byte t
        | VarSingle t -> byte t
        | VarDouble t -> byte t
        | VarDecimal t -> byte t
        | _ -> notSupported()
    member this.ToInt16() =
        match this.value with
        | VarNull -> int16 0
        | VarBool t -> int16(boolI32(t))
        | VarChar t -> int16 t
        | VarInt8 t ->  int16 t
        | VarUInt8 t -> int16 t
        | VarInt16 t -> int16 t
        | VarUInt16 t -> int16 t
        | VarInt32 t -> int16 t
        | VarUInt32 t -> int16 t
        | VarInt64 t -> int16 t
        | VarUInt64 t -> int16 t
        | VarSingle t -> int16 t
        | VarDouble t -> int16 t
        | VarDecimal t -> int16 t
        | _ -> notSupported()
    member this.ToUInt16() =
        match this.value with
        | VarNull -> uint16 0
        | VarBool t -> uint16(boolI32(t))
        | VarChar t -> uint16 t
        | VarInt8 t ->  uint16 t
        | VarUInt8 t -> uint16 t
        | VarInt16 t -> uint16 t
        | VarUInt16 t -> uint16 t
        | VarInt32 t -> uint16 t
        | VarUInt32 t -> uint16 t
        | VarInt64 t -> uint16 t
        | VarUInt64 t -> uint16 t
        | VarSingle t -> uint16 t
        | VarDouble t -> uint16 t
        | VarDecimal t -> uint16 t
        | _ -> notSupported()
    member this.ToInt32() =
        match this.value with
        | VarNull -> 0
        | VarBool t -> boolI32(t)
        | VarChar t -> int t
        | VarInt8 t -> int t
        | VarUInt8 t -> int t
        | VarInt16 t -> int t
        | VarUInt16 t -> int t
        | VarInt32 t -> int t
        | VarUInt32 t -> int t
        | VarInt64 t -> int t
        | VarUInt64 t -> int t
        | VarSingle t -> int t
        | VarDouble t -> int t
        | VarDecimal t -> int t
        | VarString t -> Convert.ToInt32(t)
        | _ -> notSupported()
    member this.ToUInt32() =
        match this.value with
        | VarNull -> 0u
        | VarBool t -> boolU32(t)
        | VarChar t -> uint32 t
        | VarInt8 t -> uint32 t
        | VarUInt8 t -> uint32 t
        | VarInt16 t -> uint32 t
        | VarUInt16 t -> uint32 t
        | VarInt32 t -> uint32 t
        | VarUInt32 t -> uint32 t
        | VarInt64 t -> uint32 t
        | VarUInt64 t -> uint32 t
        | VarSingle t -> uint32 t
        | VarDouble t -> uint32 t
        | VarDecimal t -> uint32 t
        | VarString t -> Convert.ToUInt32(t)
        | _ -> notSupported()
    member this.ToInt64() =
        match this.value with
        | VarNull -> 0L
        | VarBool t -> if t then 1L else 0L
        | VarChar t -> int64 t
        | VarInt8 t -> int64 t
        | VarUInt8 t -> int64 t
        | VarInt16 t -> int64 t
        | VarUInt16 t -> int64 t
        | VarInt32 t -> int64 t
        | VarUInt32 t -> int64 t
        | VarInt64 t -> int64 t
        | VarUInt64 t -> int64 t
        | VarSingle t -> int64 t
        | VarDouble t -> int64 t
        | VarDecimal t -> int64 t
        | VarString t -> Convert.ToInt64(t)
        | _ -> notSupported()
    member this.ToUInt64() =
        match this.value with
        | VarNull -> 0UL
        | VarBool t -> if t then 1UL else 0UL
        | VarChar t -> uint64 t
        | VarInt8 t -> uint64 t
        | VarUInt8 t -> uint64 t
        | VarInt16 t -> uint64 t
        | VarUInt16 t -> uint64 t
        | VarInt32 t -> uint64 t
        | VarUInt32 t -> uint64 t
        | VarInt64 t -> uint64 t
        | VarUInt64 t -> uint64 t
        | VarSingle t -> uint64 t
        | VarDouble t -> uint64 t
        | VarDecimal t -> uint64 t
        | VarString t -> Convert.ToUInt64(t)
        | _ -> notSupported()
    member this.ToSingle() =
        match this.value with
        | VarNull -> float32 0
        | VarBool t -> if t then float32 1 else float32 0
        | VarChar t -> float32 t
        | VarInt8 t -> float32 t
        | VarUInt8 t -> float32 t
        | VarInt16 t -> float32 t
        | VarUInt16 t -> float32 t
        | VarInt32 t -> float32 t
        | VarUInt32 t -> float32 t
        | VarInt64 t -> float32 t
        | VarUInt64 t -> float32 t
        | VarSingle t -> float32 t
        | VarDouble t -> float32 t
        | VarDecimal t -> float32 t
        | VarString t -> Convert.ToSingle(t)
        | _ -> notSupported()
    member this.ToDouble() =
        match this.value with
        | VarNull -> 0.0
        | VarBool t -> if t then 1.0 else 0.0
        | VarChar t -> double t
        | VarInt8 t -> double t
        | VarUInt8 t -> double t
        | VarInt16 t -> double t
        | VarUInt16 t -> double t
        | VarInt32 t -> double t
        | VarUInt32 t -> double t
        | VarInt64 t -> double t
        | VarUInt64 t -> double t
        | VarSingle t -> double t
        | VarDouble t -> double t
        | VarDecimal t -> double t
        | VarString t -> Convert.ToDouble(t)
        | _ -> notSupported()
    member this.ToDecimal() =
        match this.value with
        | VarNull -> decimal 0
        | VarBool t -> if t then decimal 1 else decimal 0
        | VarChar t -> decimal (uint32 t)
        | VarInt8 t -> decimal t
        | VarUInt8 t -> decimal t
        | VarInt16 t -> decimal t
        | VarUInt16 t -> decimal t
        | VarInt32 t -> decimal t
        | VarUInt32 t -> decimal t
        | VarInt64 t -> decimal t
        | VarUInt64 t -> decimal t
        | VarSingle t -> decimal t
        | VarDouble t -> decimal t
        | VarDecimal t -> decimal t
        | VarString t -> Convert.ToDecimal(t)
        | _ -> notSupported()
    member this.ToObject() =
        match this.value with
        | VarNull -> null
        | VarBool t -> t :> obj
        | VarChar t -> t :> obj
        | VarInt8 t -> t :> obj
        | VarUInt8 t -> t :> obj
        | VarInt16 t -> t :> obj
        | VarUInt16 t -> t :> obj
        | VarInt32 t -> t :> obj
        | VarUInt32 t -> t :> obj
        | VarInt64 t -> t :> obj
        | VarUInt64 t -> t :> obj
        | VarSingle t -> t :> obj
        | VarDouble t -> t :> obj
        | VarDecimal t -> t :> obj
        | VarString t -> t :> obj
        | VarObject t -> t
    override this.ToString() =
        match this.value with
        | VarNull -> null
        | VarBool t -> t.ToString()
        | VarChar t -> t.ToString()
        | VarInt8 t -> t.ToString()
        | VarUInt8 t -> t.ToString()
        | VarInt16 t -> t.ToString()
        | VarUInt16 t -> t.ToString()
        | VarInt32 t -> t.ToString()
        | VarUInt32 t -> t.ToString()
        | VarInt64 t -> t.ToString()
        | VarUInt64 t -> t.ToString()
        | VarSingle t -> t.ToString()
        | VarDouble t -> t.ToString()
        | VarDecimal t -> t.ToString()
        | VarString t -> t.ToString()
        | VarObject t -> t.ToString()
    member this.ToUnsigned() =
        match this.value with
        | VarBool x -> Variant(VarUInt32(boolU32(x)))
        | VarChar x -> Variant(VarUInt32(uint32 x))
        | VarInt8 x -> Variant(VarUInt32(uint32 x))
        | VarInt16 x -> Variant(VarUInt32(uint32 x))
        | VarInt32 x -> Variant(VarUInt32(uint32 x))
        | VarInt64 x -> Variant(VarUInt64(uint64 x))
        | _ -> this
    member this.ChangeType(t: TypeCode) =
        match t with
        | TypeCode.Boolean -> Variant(VarBool(this.ToBoolean()))
        | TypeCode.Char -> Variant(VarChar(this.ToChar()))
        | TypeCode.SByte -> Variant(VarInt8(this.ToSByte()))
        | TypeCode.Byte -> Variant(VarUInt8(this.ToByte()))
        | TypeCode.Int16 -> Variant(VarInt16(this.ToInt16()))
        | TypeCode.UInt16 -> Variant(VarUInt16(this.ToUInt16()))
        | TypeCode.Int32 -> Variant(VarInt32(this.ToInt32()))
        | TypeCode.UInt32 -> Variant(VarUInt32(this.ToUInt32()))
        | TypeCode.Int64 -> Variant(VarInt64(this.ToInt64()))
        | TypeCode.UInt64 -> Variant(VarUInt64(this.ToUInt64()))
        | TypeCode.Single -> Variant(VarSingle(this.ToSingle()))
        | TypeCode.Double -> Variant(VarDouble(this.ToDouble()))
        | TypeCode.Decimal -> Variant(VarDecimal(this.ToDecimal()))
        | TypeCode.String -> Variant(VarString(this.ToString()))
        | _ -> notSupported()
    static member Compare (a: Variant) (b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarNull -> 0
            | _ -> -1
        | VarBool x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI32(boolI32(x) - boolI32(y))
            | VarChar y -> cmpI32(boolI32(x) - int32 y)
            | VarInt8 y -> cmpI32(boolI32(x) - int32 y)
            | VarUInt8 y -> cmpI32(boolI32(x) - int32 y)
            | VarInt32 y -> cmpI32(boolI32(x) - int32 y)
            | VarUInt32 y -> cmpU32(boolU32(x) - y)
            | VarInt64 y -> cmpI64(boolI64(x) - y)
            | VarUInt64 y -> cmpU64(boolU64(x) - y)
            | VarSingle y -> cmpSingle(boolSingle(x) - y)
            | VarDouble y -> cmpDouble(boolDouble(x) - y)
            | VarDecimal y -> cmpDecimal(boolDecimal(x) - y)
            | VarString _ -> cmpDecimal(boolDecimal(x) - b.ToDecimal())
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI32(int32 x - boolI32(y))
            | VarChar y -> cmpI32(int32 x - int32 y)
            | VarInt8 y -> cmpI32(int32 x - int32 y)
            | VarUInt8 y -> cmpI32(int32 x - int32 y)
            | VarInt32 y -> cmpI32(int32 x - int32 y)
            | VarUInt32 y -> cmpU32(uint32 x - y)
            | VarInt64 y -> cmpI64(int64 x - y)
            | VarUInt64 y -> cmpU64(uint64 x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal(int32 x) - y)
            | VarString _ -> cmpDecimal(decimal(int32 x) - b.ToDecimal())
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI32(int32 x - boolI32(y))
            | VarChar y -> cmpI32(int32 x - int32 y)
            | VarInt8 y -> cmpI32(int32 x - int32 y)
            | VarUInt8 y -> cmpI32(int32 x - int32 y)
            | VarInt32 y -> cmpI32(int32 x - int32 y)
            | VarUInt32 y -> cmpU32(uint32 x - y)
            | VarInt64 y -> cmpI64(int64 x - y)
            | VarUInt64 y -> cmpU64(uint64 x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI32(int32 x - boolI32(y))
            | VarChar y -> cmpI32(int32 x - int32 y)
            | VarInt8 y -> cmpI32(int32 x - int32 y)
            | VarUInt8 y -> cmpI32(int32 x - int32 y)
            | VarInt32 y -> cmpI32(int32 x - int32 y)
            | VarUInt32 y -> cmpU32(uint32 x - y)
            | VarInt64 y -> cmpI64(int64 x - y)
            | VarUInt64 y -> cmpU64(uint64 x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI32(int32 x - boolI32(y))
            | VarChar y -> cmpI32(int32 x - int32 y)
            | VarInt8 y -> cmpI32(int32 x - int32 y)
            | VarUInt8 y -> cmpI32(int32 x - int32 y)
            | VarInt32 y -> cmpI32(int32 x - int32 y)
            | VarUInt32 y -> cmpU32(uint32 x - y)
            | VarInt64 y -> cmpI64(int64 x - y)
            | VarUInt64 y -> cmpU64(uint64 x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI32(int32 x - boolI32(y))
            | VarChar y -> cmpI32(int32 x - int32 y)
            | VarInt8 y -> cmpI32(int32 x - int32 y)
            | VarUInt8 y -> cmpI32(int32 x - int32 y)
            | VarInt32 y -> cmpI32(int32 x - int32 y)
            | VarUInt32 y -> cmpU32(uint32 x - y)
            | VarInt64 y -> cmpI64(int64 x - y)
            | VarUInt64 y -> cmpU64(uint64 x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI32(x - boolI32(y))
            | VarChar y -> cmpI32(x - int32 y)
            | VarInt8 y -> cmpI32(x - int32 y)
            | VarUInt8 y -> cmpI32(x - int32 y)
            | VarInt32 y -> cmpI32(x - int32 y)
            | VarUInt32 y -> cmpU32(uint32 x - y)
            | VarInt64 y -> cmpI64(int64 x - y)
            | VarUInt64 y -> cmpU64(uint64 x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpU32(x - boolU32(y))
            | VarChar y -> cmpU32(x - uint32 y)
            | VarInt8 y -> cmpU32(x - uint32 y)
            | VarUInt8 y -> cmpU32(x - uint32 y)
            | VarInt32 y -> cmpU32(x - uint32 y)
            | VarUInt32 y -> cmpU32(x - y)
            | VarInt64 y -> cmpI64(int64 x - y)
            | VarUInt64 y -> cmpU64(uint64 x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI64(x - boolI64(y))
            | VarChar y -> cmpI64(x - int64 y)
            | VarInt8 y -> cmpI64(x - int64 y)
            | VarUInt8 y -> cmpI64(x - int64 y)
            | VarInt32 y -> cmpI64(x - int64 y)
            | VarUInt32 y -> cmpI64(x - int64 y)
            | VarInt64 y -> cmpI64(x - y)
            | VarUInt64 y -> cmpU64(uint64 x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpI64(int64(x - boolU64(y)))
            | VarChar y -> cmpU64(x - uint64 y)
            | VarInt8 y -> cmpU64(x - uint64 y)
            | VarUInt8 y -> cmpU64(x - uint64 y)
            | VarInt32 y -> cmpU64(x - uint64 y)
            | VarUInt32 y -> cmpU64(x - uint64 y)
            | VarInt64 y -> cmpU64(x - uint64 y)
            | VarUInt64 y -> cmpU64(x - y)
            | VarSingle y -> cmpSingle(float32 x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpSingle(x - boolSingle(y))
            | VarChar y -> cmpSingle(x - float32 y)
            | VarInt8 y -> cmpSingle(x - float32 y)
            | VarUInt8 y -> cmpSingle(x - float32 y)
            | VarInt32 y -> cmpSingle(x - float32 y)
            | VarUInt32 y -> cmpSingle(x - float32 y)
            | VarInt64 y -> cmpSingle(x - float32 y)
            | VarUInt64 y -> cmpSingle(x - float32 y)
            | VarSingle y -> cmpSingle(x - y)
            | VarDouble y -> cmpDouble(double x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpDouble(x - boolDouble(y))
            | VarChar y -> cmpDouble(x - double y)
            | VarInt8 y -> cmpDouble(x - double y)
            | VarUInt8 y -> cmpDouble(x - double y)
            | VarInt32 y -> cmpDouble(x - double y)
            | VarUInt32 y -> cmpDouble(x - double y)
            | VarInt64 y -> cmpDouble(x - double y)
            | VarUInt64 y -> cmpDouble(x - double y)
            | VarSingle y -> cmpDouble(x - double y)
            | VarDouble y -> cmpDouble(x - y)
            | VarDecimal y -> cmpDecimal(decimal x - y)
            | VarString _ -> cmpDecimal(decimal x - b.ToDecimal())
            | _ -> notSupported()
        | VarDecimal x -> 
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpDecimal(x - boolDecimal(y))
            | VarChar y -> cmpDecimal(x - decimal(uint32 y))
            | VarInt8 y -> cmpDecimal(x - decimal y)
            | VarUInt8 y -> cmpDecimal(x - decimal y)
            | VarInt32 y -> cmpDecimal(x - decimal y)
            | VarUInt32 y -> cmpDecimal(x - decimal y)
            | VarInt64 y -> cmpDecimal(x - decimal y)
            | VarUInt64 y -> cmpDecimal(x - decimal y)
            | VarSingle y -> cmpDecimal(x - decimal y)
            | VarDouble y -> cmpDecimal(x - decimal y)
            | VarDecimal y -> cmpDecimal(x - y)
            | VarString _ -> cmpDecimal(x - b.ToDecimal())
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> 1
            | VarBool y -> cmpDecimal(a.ToDecimal() - boolDecimal(y))
            | VarChar y -> cmpDecimal(a.ToDecimal() - decimal(uint32 y))
            | VarInt8 y -> cmpDecimal(a.ToDecimal() - decimal y)
            | VarUInt8 y -> cmpDecimal(a.ToDecimal() - decimal y)
            | VarInt32 y -> cmpDecimal(a.ToDecimal() - decimal y)
            | VarUInt32 y -> cmpDecimal(a.ToDecimal() - decimal y)
            | VarInt64 y -> cmpDecimal(a.ToDecimal() - decimal y)
            | VarUInt64 y -> cmpDecimal(a.ToDecimal() - decimal y)
            | VarSingle y -> cmpDecimal(a.ToDecimal() - decimal y)
            | VarDouble y -> cmpDecimal(a.ToDecimal() - decimal y)
            | VarDecimal y -> cmpDecimal(a.ToDecimal() - y)
            | VarString _ -> cmpDecimal(a.ToDecimal() - b.ToDecimal())
            | _ -> notSupported()
        | _ -> notSupported()
    interface IComparable<Variant> with
        member this.CompareTo(v: Variant) = Variant.Compare this v
    interface IComparable with
        member this.CompareTo(v: obj) = Variant.Compare this (v :?> Variant)
    override this.Equals(v: obj) =
        if v :? Variant then Variant.Compare this (v :?> Variant) = 0
        else false
    override this.GetHashCode() =
        match this.value with
        | VarNull -> 0
        | VarBool x -> x.GetHashCode()
        | VarChar x -> x.GetHashCode()
        | VarInt8 x -> x.GetHashCode()
        | VarUInt8 x -> x.GetHashCode()
        | VarInt16 x -> x.GetHashCode()
        | VarUInt16 x -> x.GetHashCode()
        | VarInt32 x -> x.GetHashCode()
        | VarUInt32 x -> x.GetHashCode()
        | VarInt64 x -> x.GetHashCode()
        | VarUInt64 x -> x.GetHashCode()
        | VarSingle x -> x.GetHashCode()
        | VarDouble x -> x.GetHashCode()
        | VarDecimal x -> x.GetHashCode()
        | VarString x -> x.GetHashCode()
        | VarObject x -> x.GetHashCode()
    // <, >, <=, >= operators
    static member (=) (a: Variant, b: Variant) = Variant.Compare a b = 0
    static member (<>) (a: Variant, b: Variant) =  Variant.Compare a b <> 0
    static member (<) (a: Variant, b: Variant) = Variant.Compare a b < 0
    static member (>) (a: Variant, b: Variant) = Variant.Compare a b > 0
    static member (<=) (a: Variant, b: Variant) = Variant.Compare a b <= 0
    static member (>=) (a: Variant, b: Variant) = Variant.Compare a b >= 0
    // unary minus
    static member op_Negate (a: Variant) =
        match a.value with
        | VarNull -> a
        | VarBool x -> Variant(VarInt32(-boolI32(x)))
        | VarChar x -> Variant(VarInt32(-(int32 x)))
        | VarInt8 x -> Variant(VarInt8(-x))
        | VarUInt8 x -> Variant(VarInt32(-(int32 x)))
        | VarInt16 x -> Variant(VarInt16(-x))
        | VarUInt16 x -> Variant(VarInt32(-(int32 x)))
        | VarInt32 x -> Variant(VarInt32(-x))
        | VarUInt32 x -> Variant(VarInt32(-(int32 x)))
        | VarInt64 x -> Variant(VarInt64(-x))
        | VarUInt64 x -> Variant(VarInt64(-(int64 x)))
        | VarSingle x -> Variant(VarSingle(-x))
        | VarDouble x -> Variant(VarDouble(-x))
        | VarDecimal x -> Variant(VarDecimal(-x))
        | VarString _ -> Variant(VarDecimal(-a.ToDecimal()))
        | _ -> notSupported()
    // operator +
    static member (+) (a: Variant, b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarString _ -> Variant(VarDecimal(b.ToDecimal()))
            | VarObject _ -> notSupported()
            | _ -> b
        | VarBool x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(boolI32(x) + boolI32(y)))
            | VarChar y -> Variant(VarInt32(boolI32(x) + int32 y))
            | VarInt8 y -> Variant(VarInt32(boolI32(x) + int32 y))
            | VarUInt8 y -> Variant(VarUInt32(boolU32(x) + uint32 y))
            | VarInt16 y -> Variant(VarInt32(boolI32(x) + int32 y))
            | VarUInt16 y -> Variant(VarUInt32(boolU32(x) + uint32 y))
            | VarInt32 y -> Variant(VarInt32(boolI32(x) + int32 y))
            | VarUInt32 y -> Variant(VarUInt32(boolU32(x) + y))
            | VarInt64 y -> Variant(VarInt64(boolI64(x) + y))
            | VarUInt64 y -> Variant(VarUInt64(boolU64(x) + y))
            | VarSingle y -> Variant(VarSingle(boolSingle(x) + y))
            | VarDouble y -> Variant(VarDouble(boolDouble(x) + y))
            | VarDecimal y -> Variant(VarDecimal(boolDecimal(x) + y))
            | VarString _ -> Variant(VarDecimal(boolDecimal(x) + b.ToDecimal()))
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(uint32 x + boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x + y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x + y))
            | VarInt64 y -> Variant(VarInt64(int64 x + y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x + y))
            | VarSingle y -> Variant(VarSingle(float32 x + y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal(int32 x) + y))
            | VarString _ -> Variant(VarDecimal(decimal(int32 x) + b.ToDecimal()))
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x + boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x + int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x + int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x + int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x + y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x + y))
            | VarInt64 y -> Variant(VarInt64(int64 x + y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x + y))
            | VarSingle y -> Variant(VarSingle(float32 x + y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(uint32 x + boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x + y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x + y))
            | VarInt64 y -> Variant(VarInt64(int64 x + y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x + y))
            | VarSingle y -> Variant(VarSingle(float32 x + y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x + boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x + int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x + int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x + int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x + y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x + y))
            | VarInt64 y -> Variant(VarInt64(int64 x + y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x + y))
            | VarSingle y -> Variant(VarSingle(float32 x + y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(uint32 x + boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x + int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x + uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x + y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x + y))
            | VarInt64 y -> Variant(VarInt64(int64 x + y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x + y))
            | VarSingle y -> Variant(VarSingle(float32 x + y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(x + boolI32(y)))
            | VarChar y -> Variant(VarInt32(x + int32 y))
            | VarInt8 y -> Variant(VarInt32(x + int32 y))
            | VarUInt8 y -> Variant(VarInt32(x + int32 y))
            | VarInt16 y -> Variant(VarInt32(x + int32 y))
            | VarUInt16 y -> Variant(VarInt32(x + int32 y))
            | VarInt32 y -> Variant(VarInt32(x + y))
            | VarUInt32 y -> Variant(VarInt32(x + int32 y))
            | VarInt64 y -> Variant(VarInt64(int64 x + y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x + y))
            | VarSingle y -> Variant(VarSingle(float32 x + y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(x + boolU32(y)))
            | VarChar y -> Variant(VarUInt32(x + uint32 y))
            | VarInt8 y -> Variant(VarUInt32(x + uint32 y))
            | VarUInt8 y -> Variant(VarUInt32(x + uint32 y))
            | VarInt16 y -> Variant(VarUInt32(x + uint32 y))
            | VarUInt16 y -> Variant(VarUInt32(x + uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x + y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x + y))
            | VarInt64 y -> Variant(VarInt64(int64 x + y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x + y))
            | VarSingle y -> Variant(VarSingle(float32 x + y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt64(x + boolI64(y)))
            | VarChar y -> Variant(VarInt64(x + int64 y))
            | VarInt8 y -> Variant(VarInt64(x + int64 y))
            | VarUInt8 y -> Variant(VarInt64(x + int64 y))
            | VarInt16 y -> Variant(VarInt64(x + int64 y))
            | VarUInt16 y -> Variant(VarInt64(x + int64 y))
            | VarInt32 y -> Variant(VarInt64(x + int64 y))
            | VarUInt32 y -> Variant(VarInt64(x + int64 y))
            | VarInt64 y -> Variant(VarInt64(x + y))
            | VarUInt64 y -> Variant(VarInt64(x + int64 y))
            | VarSingle y -> Variant(VarDouble(double x + double y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt64(x + boolU64(y)))
            | VarChar y -> Variant(VarUInt64(x + uint64 y))
            | VarInt8 y -> Variant(VarInt64(int64 x + int64 y))
            | VarUInt8 y -> Variant(VarUInt64(x + uint64 y))
            | VarInt16 y -> Variant(VarInt64(int64 x + int64 y))
            | VarUInt16 y -> Variant(VarUInt64(x + uint64 y))
            | VarInt32 y -> Variant(VarInt64(int64 x + int64 y))
            | VarUInt32 y -> Variant(VarUInt64(x + uint64 y))
            | VarInt64 y -> Variant(VarInt64(int64 x + y))
            | VarUInt64 y -> Variant(VarUInt64(x + y))
            | VarSingle y -> Variant(VarDouble(double x + double y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarSingle(x + boolSingle(y)))
            | VarChar y -> Variant(VarSingle(x + float32 y))
            | VarInt8 y -> Variant(VarSingle(x + float32 y))
            | VarUInt8 y -> Variant(VarSingle(x + float32 y))
            | VarInt16 y -> Variant(VarSingle(x + float32 y))
            | VarUInt16 y -> Variant(VarSingle(x + float32 y))
            | VarInt32 y -> Variant(VarSingle(x + float32 y))
            | VarUInt32 y -> Variant(VarSingle(x + float32 y))
            | VarInt64 y -> Variant(VarSingle(x + float32 y))
            | VarUInt64 y -> Variant(VarSingle(x + float32 y))
            | VarSingle y -> Variant(VarSingle(x + y))
            | VarDouble y -> Variant(VarDouble(double x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarDouble(x + boolDouble(y)))
            | VarChar y -> Variant(VarDouble(x + double y))
            | VarInt8 y -> Variant(VarDouble(x + double y))
            | VarUInt8 y -> Variant(VarDouble(x + double y))
            | VarInt16 y -> Variant(VarDouble(x + double y))
            | VarUInt16 y -> Variant(VarDouble(x + double y))
            | VarInt32 y -> Variant(VarDouble(x + double y))
            | VarUInt32 y -> Variant(VarDouble(x + double y))
            | VarInt64 y -> Variant(VarDouble(x + double y))
            | VarUInt64 y -> Variant(VarDouble(x + double y))
            | VarSingle y -> Variant(VarDouble(x + double y))
            | VarDouble y -> Variant(VarDouble(x + y))
            | VarDecimal y -> Variant(VarDecimal(decimal x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarDecimal x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarDecimal(x + boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(x + decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(x + decimal y))
            | VarUInt8 y -> Variant(VarDecimal(x + decimal y))
            | VarInt16 y -> Variant(VarDecimal(x + decimal y))
            | VarUInt16 y -> Variant(VarDecimal(x + decimal y))
            | VarInt32 y -> Variant(VarDecimal(x + decimal y))
            | VarUInt32 y -> Variant(VarDecimal(x + decimal y))
            | VarInt64 y -> Variant(VarDecimal(x + decimal y))
            | VarUInt64 y -> Variant(VarDecimal(x + decimal y))
            | VarSingle y -> Variant(VarDecimal(x + decimal y))
            | VarDouble y -> Variant(VarDecimal(x + decimal y))
            | VarDecimal y -> Variant(VarDecimal(x + y))
            | VarString _ -> Variant(VarDecimal(decimal x + b.ToDecimal()))
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> Variant(VarDecimal(a.ToDecimal()))
            | VarBool y -> Variant(VarDecimal(a.ToDecimal() + boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(a.ToDecimal() + decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarUInt8 y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarInt16 y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarUInt16 y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarInt32 y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarUInt32 y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarInt64 y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarUInt64 y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarSingle y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarDouble y -> Variant(VarDecimal(a.ToDecimal() + decimal y))
            | VarDecimal y -> Variant(VarDecimal(a.ToDecimal() + y))
            | VarString _ -> Variant(VarDecimal(a.ToDecimal() + b.ToDecimal()))
            | _ -> notSupported()
        | _ -> notSupported()
    // operator -
    static member (-) (a: Variant, b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarString _ -> Variant(VarDecimal(-b.ToDecimal()))
            | VarObject _ -> notSupported()
            | _ -> Variant.op_Negate(b)
        | VarBool x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(boolI32(x) - boolI32(y)))
            | VarChar y -> Variant(VarInt32(boolI32(x) - int32 y))
            | VarInt8 y -> Variant(VarInt32(boolI32(x) - int32 y))
            | VarUInt8 y -> Variant(VarInt32(boolI32(x) - int32 y))
            | VarInt16 y -> Variant(VarInt32(boolI32(x) - int32 y))
            | VarUInt16 y -> Variant(VarInt32(boolI32(x) - int32 y))
            | VarInt32 y -> Variant(VarInt32(boolI32(x) - int32 y))
            | VarUInt32 y -> Variant(VarUInt32(boolU32(x) - y))
            | VarInt64 y -> Variant(VarInt64(boolI64(x) - y))
            | VarUInt64 y -> Variant(VarUInt64(boolU64(x) - y))
            | VarSingle y -> Variant(VarSingle(boolSingle(x) - y))
            | VarDouble y -> Variant(VarDouble(boolDouble(x) - y))
            | VarDecimal y -> Variant(VarDecimal(boolDecimal(x) - y))
            | VarString _ -> Variant(VarDecimal(boolDecimal(x) - b.ToDecimal()))
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x - boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x - y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x - y))
            | VarInt64 y -> Variant(VarInt64(int64 x - y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x - y))
            | VarSingle y -> Variant(VarSingle(float32 x - y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal(int32 x) - y))
            | VarString _ -> Variant(VarDecimal(decimal(int32 x) - b.ToDecimal()))
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x - boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x - y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x - y))
            | VarInt64 y -> Variant(VarInt64(int64 x - y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x - y))
            | VarSingle y -> Variant(VarSingle(float32 x - y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x - boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x - y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x - y))
            | VarInt64 y -> Variant(VarInt64(int64 x - y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x - y))
            | VarSingle y -> Variant(VarSingle(float32 x - y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x - boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x - y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x - y))
            | VarInt64 y -> Variant(VarInt64(int64 x - y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x - y))
            | VarSingle y -> Variant(VarSingle(float32 x - y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x - boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x - int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x - y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x - y))
            | VarInt64 y -> Variant(VarInt64(int64 x - y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x - y))
            | VarSingle y -> Variant(VarSingle(float32 x - y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(x - boolI32(y)))
            | VarChar y -> Variant(VarInt32(x - int32 y))
            | VarInt8 y -> Variant(VarInt32(x - int32 y))
            | VarUInt8 y -> Variant(VarInt32(x - int32 y))
            | VarInt16 y -> Variant(VarInt32(x - int32 y))
            | VarUInt16 y -> Variant(VarInt32(x - int32 y))
            | VarInt32 y -> Variant(VarInt32(x - y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x - y))
            | VarInt64 y -> Variant(VarInt64(int64 x - y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x - y))
            | VarSingle y -> Variant(VarSingle(float32 x - y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(x - boolU32(y)))
            | VarChar y -> Variant(VarUInt32(x - uint32 y))
            | VarInt8 y -> Variant(VarUInt32(x - uint32 y))
            | VarUInt8 y -> Variant(VarUInt32(x - uint32 y))
            | VarInt16 y -> Variant(VarUInt32(x - uint32 y))
            | VarUInt16 y -> Variant(VarUInt32(x - uint32 y))
            | VarInt32 y -> Variant(VarUInt32(x - uint32 y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x - y))
            | VarInt64 y -> Variant(VarInt64(int64 x - y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x - y))
            | VarSingle y -> Variant(VarSingle(float32 x - y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt64(x - boolI64(y)))
            | VarChar y -> Variant(VarInt64(x - int64 y))
            | VarInt8 y -> Variant(VarInt64(x - int64 y))
            | VarUInt8 y -> Variant(VarInt64(x - int64 y))
            | VarInt16 y -> Variant(VarInt64(x - int64 y))
            | VarUInt16 y -> Variant(VarInt64(x - int64 y))
            | VarInt32 y -> Variant(VarInt64(x - int64 y))
            | VarUInt32 y -> Variant(VarInt64(x - int64 y))
            | VarInt64 y -> Variant(VarInt64(x - y))
            | VarUInt64 y -> Variant(VarInt64(x - int64 y))
            | VarSingle y -> Variant(VarDouble(double x - double y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt64(int64 x - boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64 x - int64 y))
            | VarInt8 y -> Variant(VarInt64(int64 x - int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64 x - int64 y))
            | VarInt16 y -> Variant(VarInt64(int64 x - int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64 x - int64 y))
            | VarInt32 y -> Variant(VarInt64(int64 x - int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64 x - int64 y))
            | VarInt64 y -> Variant(VarInt64(int64 x - y))
            | VarUInt64 y -> Variant(VarInt64(int64 x - int64 y))
            | VarSingle y -> Variant(VarDouble(double x - double y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarSingle(x - boolSingle(y)))
            | VarChar y -> Variant(VarSingle(x - float32 y))
            | VarInt8 y -> Variant(VarSingle(x - float32 y))
            | VarUInt8 y -> Variant(VarSingle(x - float32 y))
            | VarInt16 y -> Variant(VarSingle(x - float32 y))
            | VarUInt16 y -> Variant(VarSingle(x - float32 y))
            | VarInt32 y -> Variant(VarSingle(x - float32 y))
            | VarUInt32 y -> Variant(VarSingle(x - float32 y))
            | VarInt64 y -> Variant(VarSingle(x - float32 y))
            | VarUInt64 y -> Variant(VarSingle(x - float32 y))
            | VarSingle y -> Variant(VarSingle(x - y))
            | VarDouble y -> Variant(VarDouble(double x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarDouble(x - boolDouble(y)))
            | VarChar y -> Variant(VarDouble(x - double y))
            | VarInt8 y -> Variant(VarDouble(x - double y))
            | VarUInt8 y -> Variant(VarDouble(x - double y))
            | VarInt16 y -> Variant(VarDouble(x - double y))
            | VarUInt16 y -> Variant(VarDouble(x - double y))
            | VarInt32 y -> Variant(VarDouble(x - double y))
            | VarUInt32 y -> Variant(VarDouble(x - double y))
            | VarInt64 y -> Variant(VarDouble(x - double y))
            | VarUInt64 y -> Variant(VarDouble(x - double y))
            | VarSingle y -> Variant(VarDouble(x - double y))
            | VarDouble y -> Variant(VarDouble(x - y))
            | VarDecimal y -> Variant(VarDecimal(decimal x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarDecimal x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarDecimal(x - boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(x - decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(x - decimal y))
            | VarUInt8 y -> Variant(VarDecimal(x - decimal y))
            | VarInt16 y -> Variant(VarDecimal(x - decimal y))
            | VarUInt16 y -> Variant(VarDecimal(x - decimal y))
            | VarInt32 y -> Variant(VarDecimal(x - decimal y))
            | VarUInt32 y -> Variant(VarDecimal(x - decimal y))
            | VarInt64 y -> Variant(VarDecimal(x - decimal y))
            | VarUInt64 y -> Variant(VarDecimal(x - decimal y))
            | VarSingle y -> Variant(VarDecimal(x - decimal y))
            | VarDouble y -> Variant(VarDecimal(x - decimal y))
            | VarDecimal y -> Variant(VarDecimal(x - y))
            | VarString _ -> Variant(VarDecimal(decimal x - b.ToDecimal()))
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> Variant(VarDecimal(a.ToDecimal()))
            | VarBool y -> Variant(VarDecimal(a.ToDecimal() - boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(a.ToDecimal() - decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarUInt8 y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarInt16 y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarUInt16 y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarInt32 y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarUInt32 y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarInt64 y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarUInt64 y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarSingle y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarDouble y -> Variant(VarDecimal(a.ToDecimal() - decimal y))
            | VarDecimal y -> Variant(VarDecimal(a.ToDecimal() - y))
            | VarString _ -> Variant(VarDecimal(a.ToDecimal() - b.ToDecimal()))
            | _ -> notSupported()
        | _ -> notSupported()
    // operator *
    static member (*) (a: Variant, b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarString _ -> Variant(VarDecimal(decimal 0 * b.ToDecimal()))
            | VarObject _ -> notSupported()
            | _ -> Variant(VarInt32(0))
        | VarBool x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(boolI32(x) * boolI32(y)))
            | VarChar y -> Variant(VarUInt32(boolU32(x) * uint32 y))
            | VarInt8 y -> Variant(VarInt32(boolI32(x) * int32 y))
            | VarUInt8 y -> Variant(VarUInt32(boolU32(x) * uint32 y))
            | VarInt16 y -> Variant(VarInt32(boolI32(x) * int32 y))
            | VarUInt16 y -> Variant(VarUInt32(boolU32(x) * uint32 y))
            | VarInt32 y -> Variant(VarInt32(boolI32(x) * int32 y))
            | VarUInt32 y -> Variant(VarUInt32(boolU32(x) * y))
            | VarInt64 y -> Variant(VarInt64(boolI64(x) * y))
            | VarUInt64 y -> Variant(VarUInt64(boolU64(x) * y))
            | VarSingle y -> Variant(VarSingle(boolSingle(x) * y))
            | VarDouble y -> Variant(VarDouble(boolDouble(x) * y))
            | VarDecimal y -> Variant(VarDecimal(boolDecimal(x) * y))
            | VarString _ -> Variant(VarDecimal(boolDecimal(x) * b.ToDecimal()))
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarUInt32(uint32 x * boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x * y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x * y))
            | VarInt64 y -> Variant(VarInt64(int64 x * y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x * y))
            | VarSingle y -> Variant(VarSingle(float32 x * y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal(int32 x) * y))
            | VarString _ -> Variant(VarDecimal(decimal(int32 x) * b.ToDecimal()))
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(int32 x * boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x * int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x * int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x * int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x * y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x * y))
            | VarInt64 y -> Variant(VarInt64(int64 x * y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x * y))
            | VarSingle y -> Variant(VarSingle(float32 x * y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarUInt32(uint32 x * boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x * y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x * y))
            | VarInt64 y -> Variant(VarInt64(int64 x * y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x * y))
            | VarSingle y -> Variant(VarSingle(float32 x * y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(int32 x * boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x * int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x * int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x * int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x * y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x * y))
            | VarInt64 y -> Variant(VarInt64(int64 x * y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x * y))
            | VarSingle y -> Variant(VarSingle(float32 x * y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarUInt32(uint32 x * boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x * uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x * y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x * y))
            | VarInt64 y -> Variant(VarInt64(int64 x * y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x * y))
            | VarSingle y -> Variant(VarSingle(float32 x * y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(x * boolI32(y)))
            | VarChar y -> Variant(VarInt32(x * int32 y))
            | VarInt8 y -> Variant(VarInt32(x * int32 y))
            | VarUInt8 y -> Variant(VarInt32(x * int32 y))
            | VarInt16 y -> Variant(VarInt32(x * int32 y))
            | VarUInt16 y -> Variant(VarInt32(x * int32 y))
            | VarInt32 y -> Variant(VarInt32(x * y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x * y))
            | VarInt64 y -> Variant(VarInt64(int64 x * y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x * y))
            | VarSingle y -> Variant(VarSingle(float32 x * y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarUInt32(x * boolU32(y)))
            | VarChar y -> Variant(VarUInt32(x * uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt8 y -> Variant(VarUInt32(x * uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x * int32 y))
            | VarUInt16 y -> Variant(VarUInt32(x * uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x * y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x * y))
            | VarInt64 y -> Variant(VarInt64(int64 x * y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x * y))
            | VarSingle y -> Variant(VarSingle(float32 x * y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt64(x * boolI64(y)))
            | VarChar y -> Variant(VarInt64(x * int64 y))
            | VarInt8 y -> Variant(VarInt64(x * int64 y))
            | VarUInt8 y -> Variant(VarInt64(x * int64 y))
            | VarInt16 y -> Variant(VarInt64(x * int64 y))
            | VarUInt16 y -> Variant(VarInt64(x * int64 y))
            | VarInt32 y -> Variant(VarInt64(x * int64 y))
            | VarUInt32 y -> Variant(VarInt64(x * int64 y))
            | VarInt64 y -> Variant(VarInt64(x * y))
            | VarUInt64 y -> Variant(VarInt64(x * int64 y))
            | VarSingle y -> Variant(VarDouble(double x * double y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt64(x * boolU64(y)))
            | VarChar y -> Variant(VarUInt64(x * uint64 y))
            | VarInt8 y -> Variant(VarInt64(int64 x * int64 y))
            | VarUInt8 y -> Variant(VarUInt64(x * uint64 y))
            | VarInt16 y -> Variant(VarInt64(int64 x * int64 y))
            | VarUInt16 y -> Variant(VarUInt64(x * uint64 y))
            | VarInt32 y -> Variant(VarInt64(int64 x * int64 y))
            | VarUInt32 y -> Variant(VarUInt64(x * uint64 y))
            | VarInt64 y -> Variant(VarInt64(int64 x * y))
            | VarUInt64 y -> Variant(VarUInt64(x * y))
            | VarSingle y -> Variant(VarDouble(double x * double y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarSingle(x * boolSingle(y)))
            | VarChar y -> Variant(VarSingle(x * float32 y))
            | VarInt8 y -> Variant(VarSingle(x * float32 y))
            | VarUInt8 y -> Variant(VarSingle(x * float32 y))
            | VarInt16 y -> Variant(VarSingle(x * float32 y))
            | VarUInt16 y -> Variant(VarSingle(x * float32 y))
            | VarInt32 y -> Variant(VarSingle(x * float32 y))
            | VarUInt32 y -> Variant(VarSingle(x * float32 y))
            | VarInt64 y -> Variant(VarSingle(x * float32 y))
            | VarUInt64 y -> Variant(VarSingle(x * float32 y))
            | VarSingle y -> Variant(VarSingle(x * y))
            | VarDouble y -> Variant(VarDouble(double x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarDouble(x * boolDouble(y)))
            | VarChar y -> Variant(VarDouble(x * double y))
            | VarInt8 y -> Variant(VarDouble(x * double y))
            | VarUInt8 y -> Variant(VarDouble(x * double y))
            | VarInt16 y -> Variant(VarDouble(x * double y))
            | VarUInt16 y -> Variant(VarDouble(x * double y))
            | VarInt32 y -> Variant(VarDouble(x * double y))
            | VarUInt32 y -> Variant(VarDouble(x * double y))
            | VarInt64 y -> Variant(VarDouble(x * double y))
            | VarUInt64 y -> Variant(VarDouble(x * double y))
            | VarSingle y -> Variant(VarDouble(x * double y))
            | VarDouble y -> Variant(VarDouble(x * y))
            | VarDecimal y -> Variant(VarDecimal(decimal x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarDecimal x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarDecimal(x * boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(x * decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(x * decimal y))
            | VarUInt8 y -> Variant(VarDecimal(x * decimal y))
            | VarInt16 y -> Variant(VarDecimal(x * decimal y))
            | VarUInt16 y -> Variant(VarDecimal(x * decimal y))
            | VarInt32 y -> Variant(VarDecimal(x * decimal y))
            | VarUInt32 y -> Variant(VarDecimal(x * decimal y))
            | VarInt64 y -> Variant(VarDecimal(x * decimal y))
            | VarUInt64 y -> Variant(VarDecimal(x * decimal y))
            | VarSingle y -> Variant(VarDecimal(x * decimal y))
            | VarDouble y -> Variant(VarDecimal(x * decimal y))
            | VarDecimal y -> Variant(VarDecimal(x * y))
            | VarString _ -> Variant(VarDecimal(decimal x * b.ToDecimal()))
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> Variant(VarDecimal(b.ToDecimal() * decimal 0))
            | VarBool y -> Variant(VarDecimal(a.ToDecimal() * boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(a.ToDecimal() * decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarUInt8 y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarInt16 y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarUInt16 y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarInt32 y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarUInt32 y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarInt64 y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarUInt64 y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarSingle y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarDouble y -> Variant(VarDecimal(a.ToDecimal() * decimal y))
            | VarDecimal y -> Variant(VarDecimal(a.ToDecimal() * y))
            | VarString _ -> Variant(VarDecimal(a.ToDecimal() * b.ToDecimal()))
            | _ -> notSupported()
        | _ -> notSupported()
    // operator /
    static member (/) (a: Variant, b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarString _ -> Variant(VarDecimal(decimal 0 * b.ToDecimal()))
            | VarObject _ -> notSupported()
            | _ -> Variant(VarInt32(0))
        | VarBool x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarInt32(boolI32(x) / boolI32(y)))
            | VarChar y -> Variant(VarUInt32(boolU32(x) / uint32 y))
            | VarInt8 y -> Variant(VarInt32(boolI32(x) / int32 y))
            | VarUInt8 y -> Variant(VarUInt32(boolU32(x) / uint32 y))
            | VarInt16 y -> Variant(VarInt32(boolI32(x) / int32 y))
            | VarUInt16 y -> Variant(VarUInt32(boolU32(x) / uint32 y))
            | VarInt32 y -> Variant(VarInt32(boolI32(x) / int32 y))
            | VarUInt32 y -> Variant(VarUInt32(boolU32(x) / y))
            | VarInt64 y -> Variant(VarInt64(boolI64(x) / y))
            | VarUInt64 y -> Variant(VarUInt64(boolU64(x) / y))
            | VarSingle y -> Variant(VarSingle(boolSingle(x) / y))
            | VarDouble y -> Variant(VarDouble(boolDouble(x) / y))
            | VarDecimal y -> Variant(VarDecimal(boolDecimal(x) / y))
            | VarString _ -> Variant(VarDecimal(boolDecimal(x) / b.ToDecimal()))
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarUInt32(uint32 x / boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x / y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x / y))
            | VarInt64 y -> Variant(VarInt64(int64 x / y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x / y))
            | VarSingle y -> Variant(VarSingle(float32 x / y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal(int32 x) / y))
            | VarString _ -> Variant(VarDecimal(decimal(int32 x) / b.ToDecimal()))
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarInt32(int32 x / boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x / int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x / int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x / int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x / y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x / y))
            | VarInt64 y -> Variant(VarInt64(int64 x / y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x / y))
            | VarSingle y -> Variant(VarSingle(float32 x / y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarUInt32(uint32 x / boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x / y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x / y))
            | VarInt64 y -> Variant(VarInt64(int64 x / y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x / y))
            | VarSingle y -> Variant(VarSingle(float32 x / y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarInt32(int32 x / boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x / int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x / int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x / int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x / y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x / y))
            | VarInt64 y -> Variant(VarInt64(int64 x / y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x / y))
            | VarSingle y -> Variant(VarSingle(float32 x / y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarUInt32(uint32 x / boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x / uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x / y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x / y))
            | VarInt64 y -> Variant(VarInt64(int64 x / y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x / y))
            | VarSingle y -> Variant(VarSingle(float32 x / y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarInt32(x / boolI32(y)))
            | VarChar y -> Variant(VarInt32(x / int32 y))
            | VarInt8 y -> Variant(VarInt32(x / int32 y))
            | VarUInt8 y -> Variant(VarInt32(x / int32 y))
            | VarInt16 y -> Variant(VarInt32(x / int32 y))
            | VarUInt16 y -> Variant(VarInt32(x / int32 y))
            | VarInt32 y -> Variant(VarInt32(x / y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x / y))
            | VarInt64 y -> Variant(VarInt64(int64 x / y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x / y))
            | VarSingle y -> Variant(VarSingle(float32 x / y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarUInt32(x / boolU32(y)))
            | VarChar y -> Variant(VarUInt32(x / uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt8 y -> Variant(VarUInt32(x / uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt16 y -> Variant(VarUInt32(x / uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x / int32 y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x / y))
            | VarInt64 y -> Variant(VarInt64(int64 x / y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x / y))
            | VarSingle y -> Variant(VarSingle(float32 x / y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt64(x / boolI64(y)))
            | VarChar y -> Variant(VarInt64(x / int64 y))
            | VarInt8 y -> Variant(VarInt64(x / int64 y))
            | VarUInt8 y -> Variant(VarInt64(x / int64 y))
            | VarInt16 y -> Variant(VarInt64(x / int64 y))
            | VarUInt16 y -> Variant(VarInt64(x / int64 y))
            | VarInt32 y -> Variant(VarInt64(x / int64 y))
            | VarUInt32 y -> Variant(VarInt64(x / int64 y))
            | VarInt64 y -> Variant(VarInt64(x / y))
            | VarUInt64 y -> Variant(VarInt64(x / int64 y))
            | VarSingle y -> Variant(VarDouble(double x / double y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt64(x / boolU64(y)))
            | VarChar y -> Variant(VarUInt64(x / uint64 y))
            | VarInt8 y -> Variant(VarInt64(int64 x / int64 y))
            | VarUInt8 y -> Variant(VarUInt64(x / uint64 y))
            | VarInt16 y -> Variant(VarInt64(int64 x / int64 y))
            | VarUInt16 y -> Variant(VarUInt64(x / uint64 y))
            | VarInt32 y -> Variant(VarInt64(int64 x / int64 y))
            | VarUInt32 y -> Variant(VarUInt64(x / uint64 y))
            | VarInt64 y -> Variant(VarInt64(int64 x / y))
            | VarUInt64 y -> Variant(VarUInt64(x / y))
            | VarSingle y -> Variant(VarDouble(double x / double y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarSingle(x / boolSingle(y)))
            | VarChar y -> Variant(VarSingle(x / float32 y))
            | VarInt8 y -> Variant(VarSingle(x / float32 y))
            | VarUInt8 y -> Variant(VarSingle(x / float32 y))
            | VarInt16 y -> Variant(VarSingle(x / float32 y))
            | VarUInt16 y -> Variant(VarSingle(x / float32 y))
            | VarInt32 y -> Variant(VarSingle(x / float32 y))
            | VarUInt32 y -> Variant(VarSingle(x / float32 y))
            | VarInt64 y -> Variant(VarSingle(x / float32 y))
            | VarUInt64 y -> Variant(VarSingle(x / float32 y))
            | VarSingle y -> Variant(VarSingle(x / y))
            | VarDouble y -> Variant(VarDouble(double x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarDouble(x / boolDouble(y)))
            | VarChar y -> Variant(VarDouble(x / double y))
            | VarInt8 y -> Variant(VarDouble(x / double y))
            | VarUInt8 y -> Variant(VarDouble(x / double y))
            | VarInt16 y -> Variant(VarDouble(x / double y))
            | VarUInt16 y -> Variant(VarDouble(x / double y))
            | VarInt32 y -> Variant(VarDouble(x / double y))
            | VarUInt32 y -> Variant(VarDouble(x / double y))
            | VarInt64 y -> Variant(VarDouble(x / double y))
            | VarUInt64 y -> Variant(VarDouble(x / double y))
            | VarSingle y -> Variant(VarDouble(x / double y))
            | VarDouble y -> Variant(VarDouble(x / y))
            | VarDecimal y -> Variant(VarDecimal(decimal x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarDecimal x ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarDecimal(x / boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(x / decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(x / decimal y))
            | VarUInt8 y -> Variant(VarDecimal(x / decimal y))
            | VarInt16 y -> Variant(VarDecimal(x / decimal y))
            | VarUInt16 y -> Variant(VarDecimal(x / decimal y))
            | VarInt32 y -> Variant(VarDecimal(x / decimal y))
            | VarUInt32 y -> Variant(VarDecimal(x / decimal y))
            | VarInt64 y -> Variant(VarDecimal(x / decimal y))
            | VarUInt64 y -> Variant(VarDecimal(x / decimal y))
            | VarSingle y -> Variant(VarDecimal(x / decimal y))
            | VarDouble y -> Variant(VarDecimal(x / decimal y))
            | VarDecimal y -> Variant(VarDecimal(x / y))
            | VarString _ -> Variant(VarDecimal(decimal x / b.ToDecimal()))
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarBool y -> Variant(VarDecimal(a.ToDecimal() / boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(a.ToDecimal() / decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarUInt8 y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarInt16 y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarUInt16 y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarInt32 y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarUInt32 y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarInt64 y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarUInt64 y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarSingle y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarDouble y -> Variant(VarDecimal(a.ToDecimal() / decimal y))
            | VarDecimal y -> Variant(VarDecimal(a.ToDecimal() / y))
            | VarString _ -> Variant(VarDecimal(a.ToDecimal() / b.ToDecimal()))
            | _ -> notSupported()
        | _ -> notSupported()
    // operator %
    static member (%) (a: Variant, b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarNull -> raise <| DivideByZeroException()
            | VarString _ -> Variant(VarDecimal(decimal 0 % b.ToDecimal()))
            | VarObject _ -> notSupported()
            | _ -> Variant(VarInt32(0))
        | VarBool x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(boolI32(x) % boolI32(y)))
            | VarChar y -> Variant(VarUInt32(boolU32(x) % uint32 y))
            | VarInt8 y -> Variant(VarInt32(boolI32(x) % int32 y))
            | VarUInt8 y -> Variant(VarUInt32(boolU32(x) % uint32 y))
            | VarInt16 y -> Variant(VarInt32(boolI32(x) % int32 y))
            | VarUInt16 y -> Variant(VarUInt32(boolU32(x) % uint32 y))
            | VarInt32 y -> Variant(VarInt32(boolI32(x) % int32 y))
            | VarUInt32 y -> Variant(VarUInt32(boolU32(x) % y))
            | VarInt64 y -> Variant(VarInt64(boolI64(x) % y))
            | VarUInt64 y -> Variant(VarUInt64(boolU64(x) % y))
            | VarSingle y -> Variant(VarSingle(boolSingle(x) % y))
            | VarDouble y -> Variant(VarDouble(boolDouble(x) % y))
            | VarDecimal y -> Variant(VarDecimal(boolDecimal(x) % y))
            | VarString _ -> Variant(VarDecimal(boolDecimal(x) % b.ToDecimal()))
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(uint32 x % boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x % y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x % y))
            | VarInt64 y -> Variant(VarInt64(int64 x % y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x % y))
            | VarSingle y -> Variant(VarSingle(float32 x % y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal(int32 x) % y))
            | VarString _ -> Variant(VarDecimal(decimal(int32 x) % b.ToDecimal()))
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x % boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x % int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x % int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x % int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x % y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x % y))
            | VarInt64 y -> Variant(VarInt64(int64 x % y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x % y))
            | VarSingle y -> Variant(VarSingle(float32 x % y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(uint32 x % boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x % y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x % y))
            | VarInt64 y -> Variant(VarInt64(int64 x % y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x % y))
            | VarSingle y -> Variant(VarSingle(float32 x % y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x % boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x % int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x % int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x % int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x % y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x % y))
            | VarInt64 y -> Variant(VarInt64(int64 x % y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x % y))
            | VarSingle y -> Variant(VarSingle(float32 x % y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(uint32 x % boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x % uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x % y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x % y))
            | VarInt64 y -> Variant(VarInt64(int64 x % y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x % y))
            | VarSingle y -> Variant(VarSingle(float32 x % y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(x % boolI32(y)))
            | VarChar y -> Variant(VarInt32(x % int32 y))
            | VarInt8 y -> Variant(VarInt32(x % int32 y))
            | VarUInt8 y -> Variant(VarInt32(x % int32 y))
            | VarInt16 y -> Variant(VarInt32(x % int32 y))
            | VarUInt16 y -> Variant(VarInt32(x % int32 y))
            | VarInt32 y -> Variant(VarInt32(x % y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x % y))
            | VarInt64 y -> Variant(VarInt64(int64 x % y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x % y))
            | VarSingle y -> Variant(VarSingle(float32 x % y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(x % boolU32(y)))
            | VarChar y -> Variant(VarUInt32(x % uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt8 y -> Variant(VarUInt32(x % uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt16 y -> Variant(VarUInt32(x % uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x % int32 y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x % y))
            | VarInt64 y -> Variant(VarInt64(int64 x % y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x % y))
            | VarSingle y -> Variant(VarSingle(float32 x % y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt64(x % boolI64(y)))
            | VarChar y -> Variant(VarInt64(x % int64 y))
            | VarInt8 y -> Variant(VarInt64(x % int64 y))
            | VarUInt8 y -> Variant(VarInt64(x % int64 y))
            | VarInt16 y -> Variant(VarInt64(x % int64 y))
            | VarUInt16 y -> Variant(VarInt64(x % int64 y))
            | VarInt32 y -> Variant(VarInt64(x % int64 y))
            | VarUInt32 y -> Variant(VarInt64(x % int64 y))
            | VarInt64 y -> Variant(VarInt64(x % y))
            | VarUInt64 y -> Variant(VarInt64(x % int64 y))
            | VarSingle y -> Variant(VarDouble(double x % double y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt64(x % boolU64(y)))
            | VarChar y -> Variant(VarUInt64(x % uint64 y))
            | VarInt8 y -> Variant(VarInt64(int64 x % int64 y))
            | VarUInt8 y -> Variant(VarUInt64(x % uint64 y))
            | VarInt16 y -> Variant(VarInt64(int64 x % int64 y))
            | VarUInt16 y -> Variant(VarUInt64(x % uint64 y))
            | VarInt32 y -> Variant(VarInt64(int64 x % int64 y))
            | VarUInt32 y -> Variant(VarUInt64(x % uint64 y))
            | VarInt64 y -> Variant(VarInt64(int64 x % y))
            | VarUInt64 y -> Variant(VarUInt64(x % y))
            | VarSingle y -> Variant(VarDouble(double x % double y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarSingle(x % boolSingle(y)))
            | VarChar y -> Variant(VarSingle(x % float32 y))
            | VarInt8 y -> Variant(VarSingle(x % float32 y))
            | VarUInt8 y -> Variant(VarSingle(x % float32 y))
            | VarInt16 y -> Variant(VarSingle(x % float32 y))
            | VarUInt16 y -> Variant(VarSingle(x % float32 y))
            | VarInt32 y -> Variant(VarSingle(x % float32 y))
            | VarUInt32 y -> Variant(VarSingle(x % float32 y))
            | VarInt64 y -> Variant(VarSingle(x % float32 y))
            | VarUInt64 y -> Variant(VarSingle(x % float32 y))
            | VarSingle y -> Variant(VarSingle(x % y))
            | VarDouble y -> Variant(VarDouble(double x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarDouble(x % boolDouble(y)))
            | VarChar y -> Variant(VarDouble(x % double y))
            | VarInt8 y -> Variant(VarDouble(x % double y))
            | VarUInt8 y -> Variant(VarDouble(x % double y))
            | VarInt16 y -> Variant(VarDouble(x % double y))
            | VarUInt16 y -> Variant(VarDouble(x % double y))
            | VarInt32 y -> Variant(VarDouble(x % double y))
            | VarUInt32 y -> Variant(VarDouble(x % double y))
            | VarInt64 y -> Variant(VarDouble(x % double y))
            | VarUInt64 y -> Variant(VarDouble(x % double y))
            | VarSingle y -> Variant(VarDouble(x % double y))
            | VarDouble y -> Variant(VarDouble(x % y))
            | VarDecimal y -> Variant(VarDecimal(decimal x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarDecimal x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarDecimal(x % boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(x % decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(x % decimal y))
            | VarUInt8 y -> Variant(VarDecimal(x % decimal y))
            | VarInt16 y -> Variant(VarDecimal(x % decimal y))
            | VarUInt16 y -> Variant(VarDecimal(x % decimal y))
            | VarInt32 y -> Variant(VarDecimal(x % decimal y))
            | VarUInt32 y -> Variant(VarDecimal(x % decimal y))
            | VarInt64 y -> Variant(VarDecimal(x % decimal y))
            | VarUInt64 y -> Variant(VarDecimal(x % decimal y))
            | VarSingle y -> Variant(VarDecimal(x % decimal y))
            | VarDouble y -> Variant(VarDecimal(x % decimal y))
            | VarDecimal y -> Variant(VarDecimal(x % y))
            | VarString _ -> Variant(VarDecimal(decimal x % b.ToDecimal()))
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarDecimal(a.ToDecimal() % boolDecimal(y)))
            | VarChar y -> Variant(VarDecimal(a.ToDecimal() % decimal(int32 y)))
            | VarInt8 y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarUInt8 y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarInt16 y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarUInt16 y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarInt32 y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarUInt32 y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarInt64 y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarUInt64 y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarSingle y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarDouble y -> Variant(VarDecimal(a.ToDecimal() % decimal y))
            | VarDecimal y -> Variant(VarDecimal(a.ToDecimal() % y))
            | VarString _ -> Variant(VarDecimal(a.ToDecimal() % b.ToDecimal()))
            | _ -> notSupported()
        | _ -> notSupported()
    // bitwise &
    static member (&&&) (a: Variant, b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarString _ -> Variant(VarInt64(0L &&& int64(roundDecimal(b.ToDecimal()))))
            | VarObject _ -> notSupported()
            | _ -> b
        | VarBool x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(boolI32(x) &&& boolI32(y)))
            | VarChar y -> Variant(VarUInt32(boolU32(x) &&& uint32 y))
            | VarInt8 y -> Variant(VarInt32(boolI32(x) &&& int32 y))
            | VarUInt8 y -> Variant(VarUInt32(boolU32(x) &&& uint32 y))
            | VarInt16 y -> Variant(VarInt32(boolI32(x) &&& int32 y))
            | VarUInt16 y -> Variant(VarUInt32(boolU32(x) &&& uint32 y))
            | VarInt32 y -> Variant(VarInt32(boolI32(x) &&& int32 y))
            | VarUInt32 y -> Variant(VarUInt32(boolU32(x) &&& y))
            | VarInt64 y -> Variant(VarInt64(boolI64(x) &&& y))
            | VarUInt64 y -> Variant(VarUInt64(boolU64(x) &&& y))
            | VarSingle y -> Variant(VarInt32(boolI32(x) &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(boolI64(x) &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(boolI64(x) &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(boolI64(x) &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(0u))
            | VarBool y -> Variant(VarUInt32(uint32 x &&& boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x &&& y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x &&& y))
            | VarInt64 y -> Variant(VarInt64(int64 x &&& y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x &&& y))
            | VarSingle y -> Variant(VarInt32(int32 x &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(int32 x &&& boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x &&& y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x &&& y))
            | VarInt64 y -> Variant(VarInt64(int64 x &&& y))
            | VarUInt64 y -> Variant(VarInt64(int64 x &&& int64 y))
            | VarSingle y -> Variant(VarInt32(int32 x &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(0u))
            | VarBool y -> Variant(VarUInt32(uint32 x &&& boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x &&& y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x &&& y))
            | VarInt64 y -> Variant(VarInt64(int64 x &&& y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x &&& y))
            | VarSingle y -> Variant(VarInt32(int32 x &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(int32 x &&& boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x &&& y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x &&& y))
            | VarInt64 y -> Variant(VarInt64(int64 x &&& y))
            | VarUInt64 y -> Variant(VarInt64(int64 x &&& int64 y))
            | VarSingle y -> Variant(VarInt32(int32 x &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(0u))
            | VarBool y -> Variant(VarUInt32(uint32 x &&& boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x &&& uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x &&& y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x &&& y))
            | VarInt64 y -> Variant(VarInt64(int64 x &&& y))
            | VarUInt64 y -> Variant(VarInt64(int64 x &&& int64 y))
            | VarSingle y -> Variant(VarInt32(int32 x &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(x &&& boolI32(y)))
            | VarChar y -> Variant(VarInt32(x &&& int32 y))
            | VarInt8 y -> Variant(VarInt32(x &&& int32 y))
            | VarUInt8 y -> Variant(VarInt32(x &&& int32 y))
            | VarInt16 y -> Variant(VarInt32(x &&& int32 y))
            | VarUInt16 y -> Variant(VarInt32(x &&& int32 y))
            | VarInt32 y -> Variant(VarInt32(x &&& y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x &&& y))
            | VarInt64 y -> Variant(VarInt64(int64 x &&& y))
            | VarUInt64 y -> Variant(VarInt64(int64 x &&& int64 y))
            | VarSingle y -> Variant(VarInt32(x &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(0u))
            | VarBool y -> Variant(VarUInt32(x &&& boolU32(y)))
            | VarChar y -> Variant(VarUInt32(x &&& uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt8 y -> Variant(VarUInt32(x &&& uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x &&& int32 y))
            | VarUInt16 y -> Variant(VarUInt32(x &&& uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x &&& y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x &&& y))
            | VarInt64 y -> Variant(VarInt64(int64 x &&& y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x &&& y))
            | VarSingle y -> Variant(VarInt32(int32 x &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> Variant(VarInt64(0L))
            | VarBool y -> Variant(VarInt64(x &&& boolI64(y)))
            | VarChar y -> Variant(VarInt64(x &&& int64 y))
            | VarInt8 y -> Variant(VarInt64(x &&& int64 y))
            | VarUInt8 y -> Variant(VarInt64(x &&& int64 y))
            | VarInt16 y -> Variant(VarInt64(x &&& int64 y))
            | VarUInt16 y -> Variant(VarInt64(x &&& int64 y))
            | VarInt32 y -> Variant(VarInt64(x &&& int64 y))
            | VarUInt32 y -> Variant(VarInt64(x &&& int64 y))
            | VarInt64 y -> Variant(VarInt64(x &&& y))
            | VarUInt64 y -> Variant(VarInt64(x &&& int64 y))
            | VarSingle y -> Variant(VarInt64(x &&& int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> Variant(VarUInt64(0UL))
            | VarBool y -> Variant(VarUInt64(uint64 x &&& boolU64(y)))
            | VarChar y -> Variant(VarUInt64(x &&& uint64 y))
            | VarInt8 y -> Variant(VarInt64(int64 x &&& int64 y))
            | VarUInt8 y -> Variant(VarUInt64(x &&& uint64 y))
            | VarInt16 y -> Variant(VarInt64(int64 x &&& int64 y))
            | VarUInt16 y -> Variant(VarUInt64(x &&& uint64 y))
            | VarInt32 y -> Variant(VarInt64(int64 x &&& int64 y))
            | VarUInt32 y -> Variant(VarUInt64(x &&& uint64 y))
            | VarInt64 y -> Variant(VarInt64(int64 x &&& y))
            | VarUInt64 y -> Variant(VarUInt64(x &&& y))
            | VarSingle y -> Variant(VarInt64(int64 x &&& int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt32(int32(roundDouble(double x)) &&& boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32(roundDouble(double x)) &&& int32 y))
            | VarInt8 y -> Variant(VarInt32(int32(roundDouble(double x)) &&& int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32(roundDouble(double x)) &&& int32 y))
            | VarInt16 y -> Variant(VarInt32(int32(roundDouble(double x)) &&& int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32(roundDouble(double x)) &&& int32 y))
            | VarInt32 y -> Variant(VarInt32(int32(roundDouble(double x)) &&& int32 y))
            | VarUInt32 y -> Variant(VarInt32(int32(roundDouble(double x)) &&& int32 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDouble(double x)) &&& y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDouble(double x)) &&& int64 y))
            | VarSingle y -> Variant(VarInt32(int32(roundDouble(double x)) &&& int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDouble(double x)) &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDouble(double x)) &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDouble(double x)) &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt64(int64(roundDouble(x)) &&& boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDouble(x)) &&& y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDouble(x)) &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDouble(x)) &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarDecimal x ->
            match b.value with
            | VarNull -> Variant(VarInt32(0))
            | VarBool y -> Variant(VarInt64(int64(roundDecimal(x)) &&& boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDecimal(x)) &&& y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDecimal(x)) &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& 0L))
            | VarBool y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) &&& int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | _ -> notSupported()
    // bitwise or
    static member (|||) (a: Variant, b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarString _ -> Variant(VarInt64(0L ||| int64(roundDecimal(b.ToDecimal()))))
            | VarObject _ -> notSupported()
            | _ -> b
        | VarBool x ->
            match b.value with
            | VarNull -> Variant(VarInt32(boolI32(x) ||| 0))
            | VarBool y -> Variant(VarInt32(boolI32(x) ||| boolI32(y)))
            | VarChar y -> Variant(VarUInt32(boolU32(x) ||| uint32 y))
            | VarInt8 y -> Variant(VarInt32(boolI32(x) ||| int32 y))
            | VarUInt8 y -> Variant(VarUInt32(boolU32(x) ||| uint32 y))
            | VarInt16 y -> Variant(VarInt32(boolI32(x) ||| int32 y))
            | VarUInt16 y -> Variant(VarUInt32(boolU32(x) ||| uint32 y))
            | VarInt32 y -> Variant(VarInt32(boolI32(x) ||| int32 y))
            | VarUInt32 y -> Variant(VarUInt32(boolU32(x) ||| y))
            | VarInt64 y -> Variant(VarInt64(boolI64(x) ||| y))
            | VarUInt64 y -> Variant(VarUInt64(boolU64(x) ||| y))
            | VarSingle y -> Variant(VarInt32(boolI32(x) ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(boolI64(x) ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(boolI64(x) ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(boolI64(x) ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(uint32 x ||| 0u))
            | VarBool y -> Variant(VarUInt32(uint32 x ||| boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ||| y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x ||| y))
            | VarInt64 y -> Variant(VarInt64(int64 x ||| y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x ||| y))
            | VarSingle y -> Variant(VarInt32(int32 x ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x ||| boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ||| y))
            | VarUInt32 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt64 y -> Variant(VarInt64(int64 x ||| y))
            | VarUInt64 y -> Variant(VarInt64(int64 x ||| int64 y))
            | VarSingle y -> Variant(VarInt32(int32 x ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(uint32 x ||| boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ||| y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x ||| y))
            | VarInt64 y -> Variant(VarInt64(int64 x ||| y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x ||| y))
            | VarSingle y -> Variant(VarInt32(int32 x ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(int32 x ||| boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ||| y))
            | VarUInt32 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt64 y -> Variant(VarInt64(int64 x ||| y))
            | VarUInt64 y -> Variant(VarInt64(int64 x ||| int64 y))
            | VarSingle y -> Variant(VarInt32(int32 x ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(uint32 x ||| boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x ||| uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ||| y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x ||| y))
            | VarInt64 y -> Variant(VarInt64(int64 x ||| y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x ||| y))
            | VarSingle y -> Variant(VarInt32(int32 x ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt32(x ||| boolI32(y)))
            | VarChar y -> Variant(VarInt32(x ||| int32 y))
            | VarInt8 y -> Variant(VarInt32(x ||| int32 y))
            | VarUInt8 y -> Variant(VarInt32(x ||| int32 y))
            | VarInt16 y -> Variant(VarInt32(x ||| int32 y))
            | VarUInt16 y -> Variant(VarInt32(x ||| int32 y))
            | VarInt32 y -> Variant(VarInt32(x ||| y))
            | VarUInt32 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarInt64 y -> Variant(VarInt64(int64 x ||| y))
            | VarUInt64 y -> Variant(VarInt64(int64 x ||| int64 y))
            | VarSingle y -> Variant(VarInt32(x ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt32(x ||| boolU32(y)))
            | VarChar y -> Variant(VarUInt32(x ||| uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt8 y -> Variant(VarUInt32(x ||| uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ||| int32 y))
            | VarUInt16 y -> Variant(VarUInt32(x ||| uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ||| y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x ||| y))
            | VarInt64 y -> Variant(VarInt64(int64 x ||| y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x ||| y))
            | VarSingle y -> Variant(VarInt32(int32 x ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarInt64(x ||| boolI64(y)))
            | VarChar y -> Variant(VarInt64(x ||| int64 y))
            | VarInt8 y -> Variant(VarInt64(x ||| int64 y))
            | VarUInt8 y -> Variant(VarInt64(x ||| int64 y))
            | VarInt16 y -> Variant(VarInt64(x ||| int64 y))
            | VarUInt16 y -> Variant(VarInt64(x ||| int64 y))
            | VarInt32 y -> Variant(VarInt64(x ||| int64 y))
            | VarUInt32 y -> Variant(VarInt64(x ||| int64 y))
            | VarInt64 y -> Variant(VarInt64(x ||| y))
            | VarUInt64 y -> Variant(VarInt64(x ||| int64 y))
            | VarSingle y -> Variant(VarInt64(x ||| int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> a
            | VarBool y -> Variant(VarUInt64(x ||| boolU64(y)))
            | VarChar y -> Variant(VarUInt64(x ||| uint64 y))
            | VarInt8 y -> Variant(VarInt64(int64 x ||| int64 y))
            | VarUInt8 y -> Variant(VarUInt64(x ||| uint64 y))
            | VarInt16 y -> Variant(VarInt64(int64 x ||| int64 y))
            | VarUInt16 y -> Variant(VarUInt64(x ||| uint64 y))
            | VarInt32 y -> Variant(VarInt64(int64 x ||| int64 y))
            | VarUInt32 y -> Variant(VarUInt64(x ||| uint64 y))
            | VarInt64 y -> Variant(VarInt64(int64 x ||| y))
            | VarUInt64 y -> Variant(VarUInt64(x ||| y))
            | VarSingle y -> Variant(VarInt64(int64 x ||| int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> Variant(VarInt32(int32(roundDouble(double x)) ||| 0))
            | VarBool y -> Variant(VarInt32(int32(roundDouble(double x)) ||| boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32(roundDouble(double x)) ||| int32 y))
            | VarInt8 y -> Variant(VarInt32(int32(roundDouble(double x)) ||| int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32(roundDouble(double x)) ||| int32 y))
            | VarInt16 y -> Variant(VarInt32(int32(roundDouble(double x)) ||| int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32(roundDouble(double x)) ||| int32 y))
            | VarInt32 y -> Variant(VarInt32(int32(roundDouble(double x)) ||| int32 y))
            | VarUInt32 y -> Variant(VarInt32(int32(roundDouble(double x)) ||| int32 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDouble(double x)) ||| y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDouble(double x)) ||| int64 y))
            | VarSingle y -> Variant(VarInt32(int32(roundDouble(double x)) ||| int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDouble(double x)) ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDouble(double x)) ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDouble(double x)) ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> Variant(VarInt64(int64(roundDouble(x)) ||| 0L))
            | VarBool y -> Variant(VarInt64(int64(roundDouble(x)) ||| boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDouble(x)) ||| y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDouble(x)) ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDouble(x)) ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarDecimal x ->
            match b.value with
            | VarNull -> Variant(VarInt64(int64(roundDecimal(x)) ||| 0L))
            | VarBool y -> Variant(VarInt64(int64(roundDecimal(x)) ||| boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDecimal(x)) ||| y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDecimal(x)) ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| 0L))
            | VarBool y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ||| int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | _ -> notSupported()
    // bitwise xor
    static member (^^^) (a: Variant, b: Variant) =
        match a.value with
        | VarNull ->
            match b.value with
            | VarString _ -> Variant(VarInt64(0L ^^^ int64(roundDecimal(b.ToDecimal()))))
            | VarObject _ -> notSupported()
            | _ -> b
        | VarBool x ->
            match b.value with
            | VarNull -> Variant(VarInt32(boolI32(x) ^^^ 0))
            | VarBool y -> Variant(VarInt32(boolI32(x) ^^^ boolI32(y)))
            | VarChar y -> Variant(VarUInt32(boolU32(x) ^^^ uint32 y))
            | VarInt8 y -> Variant(VarInt32(boolI32(x) ^^^ int32 y))
            | VarUInt8 y -> Variant(VarUInt32(boolU32(x) ^^^ uint32 y))
            | VarInt16 y -> Variant(VarInt32(boolI32(x) ^^^ int32 y))
            | VarUInt16 y -> Variant(VarUInt32(boolU32(x) ^^^ uint32 y))
            | VarInt32 y -> Variant(VarInt32(boolI32(x) ^^^ int32 y))
            | VarUInt32 y -> Variant(VarUInt32(boolU32(x) ^^^ y))
            | VarInt64 y -> Variant(VarInt64(boolI64(x) ^^^ y))
            | VarUInt64 y -> Variant(VarUInt64(boolU64(x) ^^^ y))
            | VarSingle y -> Variant(VarInt32(boolI32(x) ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(boolI64(x) ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(boolI64(x) ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(boolI64(x) ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarChar x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(uint32 x ^^^ 0u))
            | VarBool y -> Variant(VarUInt32(uint32 x ^^^ boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ^^^ y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x ^^^ y))
            | VarInt64 y -> Variant(VarInt64(int64 x ^^^ y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x ^^^ y))
            | VarSingle y -> Variant(VarInt32(int32 x ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt8 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(int32 x ^^^ 0))
            | VarBool y -> Variant(VarInt32(int32 x ^^^ boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ^^^ y))
            | VarUInt32 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt64 y -> Variant(VarInt64(int64 x ^^^ y))
            | VarUInt64 y -> Variant(VarInt64(int64 x ^^^ int64 y))
            | VarSingle y -> Variant(VarInt32(int32 x ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt8 x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(uint32 x ^^^ 0u))
            | VarBool y -> Variant(VarUInt32(uint32 x ^^^ boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ^^^ y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x ^^^ y))
            | VarInt64 y -> Variant(VarInt64(int64 x ^^^ y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x ^^^ y))
            | VarSingle y -> Variant(VarInt32(int32 x ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt16 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(int32 x ^^^ 0))
            | VarBool y -> Variant(VarInt32(int32 x ^^^ boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ^^^ y))
            | VarUInt32 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt64 y -> Variant(VarInt64(int64 x ^^^ y))
            | VarUInt64 y -> Variant(VarInt64(int64 x ^^^ int64 y))
            | VarSingle y -> Variant(VarInt32(int32 x ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt16 x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(uint32 x ^^^ 0u))
            | VarBool y -> Variant(VarUInt32(uint32 x ^^^ boolU32(y)))
            | VarChar y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt8 y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt16 y -> Variant(VarUInt32(uint32 x ^^^ uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ^^^ y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x ^^^ y))
            | VarInt64 y -> Variant(VarInt64(int64 x ^^^ y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x ^^^ y))
            | VarSingle y -> Variant(VarInt32(int32 x ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt32 x ->
            match b.value with
            | VarNull -> Variant(VarInt32(int32 x ^^^ 0))
            | VarBool y -> Variant(VarInt32(x ^^^ boolI32(y)))
            | VarChar y -> Variant(VarInt32(x ^^^ int32 y))
            | VarInt8 y -> Variant(VarInt32(x ^^^ int32 y))
            | VarUInt8 y -> Variant(VarInt32(x ^^^ int32 y))
            | VarInt16 y -> Variant(VarInt32(x ^^^ int32 y))
            | VarUInt16 y -> Variant(VarInt32(x ^^^ int32 y))
            | VarInt32 y -> Variant(VarInt32(x ^^^ y))
            | VarUInt32 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarInt64 y -> Variant(VarInt64(int64 x ^^^ y))
            | VarUInt64 y -> Variant(VarInt64(int64 x ^^^ int64 y))
            | VarSingle y -> Variant(VarInt32(x ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt32 x ->
            match b.value with
            | VarNull -> Variant(VarUInt32(x ^^^ 0u))
            | VarBool y -> Variant(VarUInt32(x ^^^ boolU32(y)))
            | VarChar y -> Variant(VarUInt32(x ^^^ uint32 y))
            | VarInt8 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt8 y -> Variant(VarUInt32(x ^^^ uint32 y))
            | VarInt16 y -> Variant(VarInt32(int32 x ^^^ int32 y))
            | VarUInt16 y -> Variant(VarUInt32(x ^^^ uint32 y))
            | VarInt32 y -> Variant(VarInt32(int32 x ^^^ y))
            | VarUInt32 y -> Variant(VarUInt32(uint32 x ^^^ y))
            | VarInt64 y -> Variant(VarInt64(int64 x ^^^ y))
            | VarUInt64 y -> Variant(VarUInt64(uint64 x ^^^ y))
            | VarSingle y -> Variant(VarInt32(int32 x ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarInt64 x ->
            match b.value with
            | VarNull -> Variant(VarInt64(x ^^^ 0L))
            | VarBool y -> Variant(VarInt64(x ^^^ boolI64(y)))
            | VarChar y -> Variant(VarInt64(x ^^^ int64 y))
            | VarInt8 y -> Variant(VarInt64(x ^^^ int64 y))
            | VarUInt8 y -> Variant(VarInt64(x ^^^ int64 y))
            | VarInt16 y -> Variant(VarInt64(x ^^^ int64 y))
            | VarUInt16 y -> Variant(VarInt64(x ^^^ int64 y))
            | VarInt32 y -> Variant(VarInt64(x ^^^ int64 y))
            | VarUInt32 y -> Variant(VarInt64(x ^^^ int64 y))
            | VarInt64 y -> Variant(VarInt64(x ^^^ y))
            | VarUInt64 y -> Variant(VarInt64(x ^^^ int64 y))
            | VarSingle y -> Variant(VarInt64(x ^^^ int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarUInt64 x ->
            match b.value with
            | VarNull -> Variant(VarUInt64(x ^^^ 0UL))
            | VarBool y -> Variant(VarUInt64(uint64 x ^^^ boolU64(y)))
            | VarChar y -> Variant(VarUInt64(x ^^^ uint64 y))
            | VarInt8 y -> Variant(VarInt64(int64 x ^^^ int64 y))
            | VarUInt8 y -> Variant(VarUInt64(x ^^^ uint64 y))
            | VarInt16 y -> Variant(VarInt64(int64 x ^^^ int64 y))
            | VarUInt16 y -> Variant(VarUInt64(x ^^^ uint64 y))
            | VarInt32 y -> Variant(VarInt64(int64 x ^^^ int64 y))
            | VarUInt32 y -> Variant(VarUInt64(x ^^^ uint64 y))
            | VarInt64 y -> Variant(VarInt64(int64 x ^^^ y))
            | VarUInt64 y -> Variant(VarUInt64(x ^^^ y))
            | VarSingle y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64 x ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64 x ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarSingle x ->
            match b.value with
            | VarNull -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ 0))
            | VarBool y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ boolI32(y)))
            | VarChar y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ int32 y))
            | VarInt8 y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ int32 y))
            | VarUInt8 y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ int32 y))
            | VarInt16 y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ int32 y))
            | VarUInt16 y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ int32 y))
            | VarInt32 y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ int32 y))
            | VarUInt32 y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ int32 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDouble(double x)) ^^^ y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDouble(double x)) ^^^ int64 y))
            | VarSingle y -> Variant(VarInt32(int32(roundDouble(double x)) ^^^ int32(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDouble(double x)) ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDouble(double x)) ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDouble(double x)) ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarDouble x ->
            match b.value with
            | VarNull -> Variant(VarInt64(int64(roundDouble(x)) ^^^ 0L))
            | VarBool y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDouble(x)) ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarDecimal x ->
            match b.value with
            | VarNull -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ 0L))
            | VarBool y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDecimal(x)) ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | VarString _ ->
            match b.value with
            | VarNull -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ 0L))
            | VarBool y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ boolI64(y)))
            | VarChar y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64 y))
            | VarInt8 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64 y))
            | VarUInt8 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64 y))
            | VarInt16 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64 y))
            | VarUInt16 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64 y))
            | VarInt32 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64 y))
            | VarUInt32 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64 y))
            | VarInt64 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ y))
            | VarUInt64 y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64 y))
            | VarSingle y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64(roundDouble(double y))))
            | VarDouble y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64(roundDouble(y))))
            | VarDecimal y -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64(roundDecimal(y))))
            | VarString _ -> Variant(VarInt64(int64(roundDecimal(a.ToDecimal())) ^^^ int64(roundDecimal(b.ToDecimal()))))
            | _ -> notSupported()
        | _ -> notSupported()
    static member op_Not (a: Variant) =
        Variant(VarBool(not (a.ToBoolean())))
    static member op_BitwiseNot (a: Variant) =
        match a.value with
        | VarNull -> VarInt32(~~~0)
        | VarBool x -> VarInt32(~~~boolI32(x))
        | VarChar x -> VarInt32(~~~int32(x))
        | VarInt8 x -> VarInt32(~~~int32(x))
        | VarUInt8 x -> VarInt32(~~~int32(x))
        | VarInt16 x -> VarInt32(~~~int32(x))
        | VarUInt16 x -> VarInt32(~~~int32(x))
        | VarInt32 x -> VarInt32(~~~x)
        | VarUInt32 x -> VarUInt32(~~~x)
        | VarInt64 x -> VarInt64(~~~x)
        | VarUInt64 x -> VarUInt64(~~~x)
        | VarSingle x -> VarSingle(float32(~~~int64(roundDouble(double x))))
        | VarDouble x -> VarDouble(double(~~~int64(roundDouble(x))))
        | VarDecimal x -> VarDecimal(decimal(~~~int64(roundDecimal(x))))
        | _ -> notSupported()
    static member (<<<) (a: Variant, shift: int) =
        match a.value with
        | VarNull -> Variant(VarInt32(0))
        | VarBool x -> Variant(VarInt32(boolI32(x) <<< shift))
        | VarChar x -> Variant(VarChar(char(uint32(x) <<< shift)))
        | VarInt8 x -> Variant(VarInt8(x <<< shift))
        | VarUInt8 x -> Variant(VarUInt8(x <<< shift))
        | VarInt16 x -> Variant(VarInt16(x <<< shift))
        | VarUInt16 x -> Variant(VarUInt16(x <<< shift))
        | VarInt32 x -> Variant(VarInt32(x <<< shift))
        | VarUInt32 x -> Variant(VarUInt32(x <<< shift))
        | VarInt64 x -> Variant(VarInt64(x <<< shift))
        | VarUInt64 x -> Variant(VarUInt64(x <<< shift))
        | VarSingle x -> Variant(VarSingle(float32(int64(roundDouble(double x)) <<< shift)))
        | VarDouble x -> Variant(VarDouble(double(int64(roundDouble(x)) <<< shift)))
        | VarDecimal x -> Variant(VarDecimal(decimal(int64(roundDecimal(x)) <<< shift)))
        | _ -> notSupported()
    static member (>>>) (a: Variant, shift: int) =
        match a.value with
        | VarNull -> Variant(VarInt32(0))
        | VarBool x -> Variant(VarInt32(boolI32(x) >>> shift))
        | VarChar x -> Variant(VarChar(char(uint32(x) >>> shift)))
        | VarInt8 x -> Variant(VarInt8(x >>> shift))
        | VarUInt8 x -> Variant(VarUInt8(x >>> shift))
        | VarInt16 x -> Variant(VarInt16(x >>> shift))
        | VarUInt16 x -> Variant(VarUInt16(x >>> shift))
        | VarInt32 x -> Variant(VarInt32(x >>> shift))
        | VarUInt32 x -> Variant(VarUInt32(x >>> shift))
        | VarInt64 x -> Variant(VarInt64(x >>> shift))
        | VarUInt64 x -> Variant(VarUInt64(x >>> shift))
        | VarSingle x -> Variant(VarSingle(float32(int64(roundDouble(double x)) >>> shift)))
        | VarDouble x -> Variant(VarDouble(double(int64(roundDouble(x)) >>> shift)))
        | VarDecimal x -> Variant(VarDecimal(decimal(int64(roundDecimal(x)) >>> shift)))
        | _ -> notSupported()

module Fint.Signature

open System
open System.IO
open System.Reflection
open Fint.IO
open Fint.CodedIndex

type SignatureKind =
    | Method = 0x00
    | Field = 0x06
    | LocalVars = 0x07
    | Property = 0x08

[<FlagsAttribute>]
type SignatureFlags =
    | GENERIC = 0x10
    | HASTHIS = 0x20
    | EXPLICITTHIS = 0x40

    //One of this values identifies method signature
    | DEFAULT = 0x00
    | C = 0x01
    | STDCALL = 0x02
    | THISCALL = 0x03
    | FASTCALL = 0x04
    | VARARG = 0x05

    | FIELD = 0x06
    | LOCAL = 0x07
    | PROPERTY = 0x08
    | TYPEMASK = 0xF

// Element types used in signatures
type ElementType =
    // Marks end of a list
    | End = 0
    | Void = 1
    | Boolean = 2
    | Char = 3
    | Int8 = 4 // System.SByte
    | UInt8 = 5 // System.Byte
    | Int16 = 6
    | UInt16 = 7
    | Int32 = 8
    | UInt32 = 9
    | Int64 = 10
    | UInt64 = 11
    | Single = 12
    | Double = 13
    | String = 14 //0x0E
    | Ptr = 15
    | ByRef = 16
    // VALUETYPE TypeDefOrRef
    | ValueType = 17
    // CLASS TypeDefOrRef
    | Class = 18
    // Generic parameter in a generic type definition, represented as number
    | Var = 19
    // MDARRAY ArrayShape
    | Array = 20
    // Generic type instantiation. Followed by type typearg-count type-1 ... type-n.
    | GenericInstantiation = 21
    // System.TypedReference
    | TypedReference = 22
    // System.IntPtr
    | IntPtr = 24
    // System.UIntPtr
    | UIntPtr = 25
    // FNPTR MethodSig
    | MethodPtr = 27
    // Shortcut for System.Object
    | Object = 28
    // Shortcut for single dimension zero lower bound array (SZARRAY Type)
    | ArraySz = 29
    // Generic parameter in a generic method definition, represented as number
    | MethodVar = 30
    // Required C modifier followed by TypeDefOrRef token
    | RequiredModifier = 31
    // Optional C modifier followed by TypeDefOrRef token
    | OptionalModifier = 32
    // INTERNAL TypeHandle
    | Internal = 33
    | ElementTypeModifier = 0x40
    // Sentinel for varargs
    | Sentinel = 0x41
    // Denotes a local variable that points at a pinned object
    | Pinned = 0x45
    // used only internally for R4 HFA types
    | R4_HFA = 0x46
    // used only internally for R8 HFA types
    | R8_HFA = 0x47
    // Indicates an argument of type System.Type.
    | CustomArgsType = 0x50
    // Used in custom attributes to specify a boxed object
    | CustomArgsBoxedObject = 0x51
    // Used in custom attributes to indicate a FIELD
    | CustomArgsField = 0x53
    // Used in custom attributes to indicate a PROPERTY
    | CustomArgsProperty = 0x54
    // Used in custom attributes to specify an enum
    | CustomArgsEnum = 0x55

type ArrayShape = {
    Rank: int;
    Sizes: int array;
    LoBounds: int array;
}

let ArraySzShape: ArrayShape = {
    Rank = 0;
    Sizes = [||];
    LoBounds = [||];
}

type TypeSignature =
    | PrimitiveTypeSig of ElementType
    | RefTypeSig of ElementType * TypeSignature
    | TypeIndexSig of ElementType * TableIndex
    | ArrayTypeSig of TypeSignature * ArrayShape
    | MethodPtrTypeSig of MethodSignature
    | TypeGenericVar of int
    | MethodGenericVar of int
    | GenericInstantiationTypeSig of TypeSignature * TypeSignature array
    | ModifierTypeSig of ElementType * TableIndex * TypeSignature
    | PinnedTypeSig of ElementType * TypeSignature
    
and MethodSignature = {
    IsProperty: bool;
    CallingConventions: CallingConventions;
    GenericParamCount: int;
    ReturnType: TypeSignature;
    Params: TypeSignature array;
}

type LocalVar = {
    Index: int;
    Type: TypeSignature;
    Name: string;
}

let decodeMethodSignatureImpl (reader: BinaryReader) decodeTypeSignature =
    let flags: SignatureFlags = enum (int(reader.ReadByte()))
    let kind = flags &&& SignatureFlags.TYPEMASK
    let isMethod = kind = SignatureFlags.PROPERTY || (kind >= SignatureFlags.DEFAULT && kind <= SignatureFlags.VARARG)
    if not isMethod then failwith "bad method signature"
    let callConv() =
        match kind with
        | SignatureFlags.VARARG -> CallingConventions.VarArgs
        | _ -> 
            let std = CallingConventions.Standard
            let hasThis =
                if (int (flags &&& SignatureFlags.HASTHIS) = 0) then std
                else CallingConventions.HasThis
            let explicitThis =
                if (int (flags &&& SignatureFlags.EXPLICITTHIS) = 0) then std
                else CallingConventions.ExplicitThis
            std ||| hasThis ||| explicitThis
    let genericParamCount = 
        if int (flags &&& SignatureFlags.GENERIC) = 0 then 0
        else ReadPackedInt(reader)
    let paramCount = ReadPackedInt(reader)
    let returnType = decodeTypeSignature(reader)
    let paramTypes = [|1..paramCount|] |> Array.map (fun _ -> decodeTypeSignature(reader))
    let result: MethodSignature = {
        IsProperty=kind = SignatureFlags.PROPERTY;
        CallingConventions=callConv();
        GenericParamCount=genericParamCount;
        ReturnType=returnType;
        Params=paramTypes;
    }
    result


let rec decodeTypeSignature (reader: BinaryReader) =
    let decodeTypeDefOrRef (reader: BinaryReader) =
        let token = ReadPackedInt(reader)
        decodeCodedIndex(typeDefOrRef, uint32 token)
    let decodeMethodSignature (reader: BinaryReader) =
        decodeMethodSignatureImpl reader decodeTypeSignature
    let e: ElementType = enum (ReadPackedInt(reader))
    match e with
        | ElementType.End
        | ElementType.Void
        | ElementType.Boolean
        | ElementType.Char
        | ElementType.Int8
        | ElementType.UInt8
        | ElementType.Int16
        | ElementType.UInt16
        | ElementType.Int32
        | ElementType.UInt32
        | ElementType.Int64
        | ElementType.UInt64
        | ElementType.Single
        | ElementType.Double
        | ElementType.String
        | ElementType.TypedReference
        | ElementType.IntPtr
        | ElementType.UIntPtr
        | ElementType.Object -> PrimitiveTypeSig(e)
        | ElementType.Ptr
        | ElementType.ByRef -> RefTypeSig(e, decodeTypeSignature(reader))
        | ElementType.ValueType
        | ElementType.Class
        | ElementType.CustomArgsEnum -> TypeIndexSig(e, decodeTypeDefOrRef(reader))
        | ElementType.ArraySz -> ArrayTypeSig(decodeTypeSignature(reader), ArraySzShape)
        | ElementType.Array ->
            let t = decodeTypeSignature(reader)
            let rank = ReadPackedInt(reader)
            let readArray n = [|1..n|] |> Array.map (fun _ -> ReadPackedInt(reader))
            let sizes = readArray (ReadPackedInt(reader))
            let loBounds = readArray (ReadPackedInt(reader))
            ArrayTypeSig(t, {Rank=rank;Sizes=sizes;LoBounds=loBounds})
        | ElementType.Var -> TypeGenericVar(ReadPackedInt(reader))
        | ElementType.MethodVar -> MethodGenericVar(ReadPackedInt(reader))
        | ElementType.MethodPtr -> MethodPtrTypeSig(decodeMethodSignature(reader))
        | ElementType.GenericInstantiation ->
            let t = decodeTypeSignature(reader)
            let n = ReadPackedInt(reader)
            let args = [|1..n|] |> Array.map (fun _ -> decodeTypeSignature(reader))
            GenericInstantiationTypeSig(t, args)
        | ElementType.RequiredModifier
        | ElementType.OptionalModifier ->
            let i = decodeTypeDefOrRef(reader)
            let t = decodeTypeSignature(reader)
            ModifierTypeSig(e, i, t)
        | ElementType.Sentinel
        | ElementType.Pinned -> PinnedTypeSig(e, decodeTypeSignature(reader))
        | _ -> failwith "not implemented"


let decodeMethodSignature (reader: BinaryReader) =
    decodeMethodSignatureImpl reader decodeTypeSignature

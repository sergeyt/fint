module Fint.MethodBody

open System
open System.Collections.Generic
open System.IO
open System.Reflection
open System.Reflection.Emit
open Fint.Enums
open Fint.IO

type SwitchTarget = int32 array

type InstructionOperand =
    | Int32 of int32
    | Int64 of int64
    | Float32 of float32
    | Float64 of double
    | StringIdx of int32
    | SwitchTarget of SwitchTarget

type Instruction =
    { opCode : OpCode
      code : InstructionCode
      operand : InstructionOperand }

// IL method body flags
[<FlagsAttribute>]
type MethodBodyFlags =
    | None = 0
    // Small Code
    | SmallFormat = 0x00
    // Tiny code format (use this code if the code size is even)
    | TinyFormat = 0x02
    // Fat code format
    | FatFormat = 0x03
    // Use this code if the code size is odd
    | TinyFormat1 = 0x06
    // Mask for extract code type
    | FormatMask = 0x07
    // Runtime call default constructor on all local vars
    | InitLocals = 0x10
    // There is another attribute after this one
    | MoreSects = 0x08

[<FlagsAttribute>]
type SEHFlags =
    | Catch = 0x00
    | Filter = 0x01
    | Finally = 0x02
    | Fault = 0x04
    | TypeMask = 0x07

type SEHBlock =
    { flags : SEHFlags
      tryOffset : int
      tryLength : int
      handlerOffset : int
      handlerLength : int
      value : int }

type MethodBody =
    { maxStackSize : int
      code : Instruction array
      sehBlocks : SEHBlock array }

let tryGetDefault (d : IDictionary<'k, 'v>) (key : 'k) (defaultValue : 'v) =
    let mutable value = defaultValue
    if d.TryGetValue(key, &value) then value
    else defaultValue

let mutable shortOpCodes : OpCode array = Array.zeroCreate (256)
let mutable longOpCodes : OpCode array = Array.zeroCreate (256)

let loadOpCodes() =
    let bf =
        BindingFlags.Public ||| BindingFlags.Static ||| BindingFlags.GetField
    let fields = typeof<OpCodes>.GetFields(bf)

    let opCodes : OpCode array =
        fields
        |> Seq.ofArray
        |> Seq.map (fun f -> (downcast (f.GetValue(null)) : OpCode))
        |> Seq.toArray

    let shortMap =
        opCodes
        |> Seq.ofArray
        |> Seq.filter (fun t -> t.Size = 1)
        |> Seq.map (fun t -> (int t.Value, t))
        |> dict

    let longMap =
        opCodes
        |> Seq.ofArray
        |> Seq.filter (fun t -> t.Size <> 1)
        |> Seq.map (fun t -> (int t.Value, t))
        |> dict

    shortOpCodes <- [ 0..255 ]
                    |> List.map (fun i -> tryGetDefault shortMap i OpCodes.Nop)
                    |> List.toArray
    longOpCodes <- [ 0..255 ]
                   |> List.map (fun i -> tryGetDefault longMap i OpCodes.Nop)
                   |> List.toArray

loadOpCodes()

let readOperand (reader : BinaryReader, opCode : OpCode) =
    match opCode.OperandType with
    | OperandType.InlineI -> Int32(reader.ReadInt32())
    | OperandType.ShortInlineI -> Int32(int (reader.ReadSByte()))
    | OperandType.InlineI8 -> Int64(reader.ReadInt64())
    | OperandType.InlineR -> Float64(reader.ReadDouble())
    | OperandType.ShortInlineR -> Float32(reader.ReadSingle())
    | OperandType.InlineString -> StringIdx(reader.ReadInt32())
    | _ -> raise (invalidOp "not implemented")

let readOpCode (reader : BinaryReader) =
    let i = int (reader.ReadByte())
    match i with
    | 0xfe -> shortOpCodes.[i]
    | _ -> longOpCodes.[int (reader.ReadByte())]

let readInstruction (reader : BinaryReader) =
    let opCode = readOpCode (reader)

    let result : Instruction =
        { opCode = opCode
          code = enum (int opCode.Value)
          operand = readOperand (reader, opCode) }
    result

let readInstructions (reader : BinaryReader, codeSize : int) =
    let code =
        seq {
            let mutable offset = 0
            while offset < codeSize do
                let pos = reader.BaseStream.Position
                let i = readInstruction (reader)
                let size = int (reader.BaseStream.Position - pos)
                offset <- offset + size
                yield i
        }
    code |> Seq.toArray

[<FlagsAttribute>]
type SectionFlags =
    | None = 0
    | EHTable = 0x01
    | OptILTable = 0x02
    | FatFormat = 0x40
    | MoreSects = 0x80

let readFatSEH (reader : BinaryReader) =
    let result : SEHBlock =
        { flags = enum (reader.ReadInt32())
          tryOffset = reader.ReadInt32()
          tryLength = reader.ReadInt32()
          handlerOffset = reader.ReadInt32()
          handlerLength = reader.ReadInt32()
          value = reader.ReadInt32() }
    result

let readTinySEH (reader : BinaryReader) =
    let result : SEHBlock =
        { flags = enum (int (reader.ReadInt16()))
          tryOffset = int (reader.ReadInt16())
          tryLength = int (reader.ReadByte())
          handlerOffset = int (reader.ReadInt16())
          handlerLength = int (reader.ReadByte())
          value = reader.ReadInt32() }
    result

let readSEHBlock (buf : byte array, fat : bool) =
    let reader = new BinaryReader(new MemoryStream(buf))
    match fat with
    | true -> readFatSEH (reader)
    | false -> readTinySEH (reader)

let readSEHBlocks (reader : BinaryReader) =
    let blocks =
        seq {
            let FatSize = 24
            let TinySize = 12
            let mutable next = true
            while next do
                // Goto 4 byte boundary (each section has to start at 4 byte boundary)
                Align4(reader)
                let header = reader.ReadUInt32()
                let sf : SectionFlags = enum (int (header &&& uint32 0xff))
                let size = int (header >>> 8) //in bytes
                let buf = ReadBytes(reader, size)

                let count() =
                    if int (sf &&& SectionFlags.OptILTable) <> 0 then (0, false)
                    elif int (sf &&& SectionFlags.FatFormat) = 0 then
                        ((size &&& 0xff) / TinySize, false)
                    elif int (sf &&& SectionFlags.EHTable) <> 0 then
                        (size / FatSize, true)
                    else (0, false)

                let (n, fat) = count()
                for _ in [ 1..n ] do
                    yield readSEHBlock (buf, fat)
                next <- int (sf &&& SectionFlags.MoreSects) <> 0
        }
    blocks |> Seq.toArray

let readMethodBody (reader : BinaryReader) =
    let lsb = uint32 (reader.ReadByte())
    let flags : MethodBodyFlags = enum (int lsb)
    let format = flags &&& MethodBodyFlags.FormatMask

    let readFatMethod() =
        let msb = uint32 (reader.ReadByte())
        let dwordMultipleSize = (msb &&& uint32 0xf0) >>> 4
        assert (dwordMultipleSize = uint32 3) // the fat header is 3 dwords
        let maxStackSize = int (reader.ReadUInt16())
        let codeSize = reader.ReadInt32()
        let localSig = reader.ReadInt32()
        let flags2 : MethodBodyFlags =
            enum (int ((msb &&& uint32 0x0f) <<< 8 ||| lsb))
        let code = readInstructions (reader, codeSize)

        let sehBlocks =
            if int (flags2 &&& MethodBodyFlags.MoreSects) <> 0 then
                readSEHBlocks (reader)
            else [||]

        let result : MethodBody =
            { maxStackSize = maxStackSize
              code = code
              sehBlocks = sehBlocks }

        result

    let readTinyMethod() =
        let codeSize = lsb >>> 2
        let code = readInstructions (reader, int codeSize)

        let result : MethodBody =
            { maxStackSize = 8
              code = code
              sehBlocks = [||] }
        result

    match format with
    | MethodBodyFlags.FatFormat -> readFatMethod()
    | MethodBodyFlags.TinyFormat -> readTinyMethod()
    | MethodBodyFlags.TinyFormat1 -> readTinyMethod()
    | _ -> raise (invalidOp "bad method format")

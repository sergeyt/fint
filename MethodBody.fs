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
    | NoOperand
    | Int32Operand of int32
    | Int64Operand of int64
    | Float32Operand of float32
    | Float64Operand of double
    | SwitchTarget of SwitchTarget
    | VarOperand of int32
    | BranchTarget of int32
    | MetadataToken of int32

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
      localSig: int
      code : Instruction array
      sehBlocks : SEHBlock array }

let tryGetDefault (d : IDictionary<'k, 'v>) (key : 'k) (defaultValue : 'v) =
    match d.TryGetValue key with
    | true, v -> v
    | false, _ -> defaultValue

let opCodes =
    lazy
        (typeof<OpCodes>
             .GetFields(BindingFlags.Public ||| BindingFlags.Static
                        ||| BindingFlags.GetField)
         |> Array.map (fun f -> (f.GetValue(null) :?> OpCode)))

let opArray size =
    let map =
        opCodes.Value
        |> Seq.ofArray
        |> Seq.filter (fun t -> t.Size = size)
        |> Seq.map (fun t -> (int t.Value &&& 0xff, t))
        |> dict
    [| 0..255 |] |> Array.map (fun i -> tryGetDefault map i OpCodes.Nop)

let shortOpCodes = lazy (opArray 1)
let longOpCodes = lazy (opArray 2)

let readOperand (reader : BinaryReader, opCode : OpCode, startPos : int64) =
    match opCode.OperandType with
    | OperandType.InlineI -> Int32Operand(reader.ReadInt32())
    | OperandType.ShortInlineI -> Int32Operand(int (reader.ReadSByte()))
    | OperandType.InlineI8 -> Int64Operand(reader.ReadInt64())
    | OperandType.InlineR -> Float64Operand(reader.ReadDouble())
    | OperandType.ShortInlineR -> Float32Operand(reader.ReadSingle())
    | OperandType.InlineString -> MetadataToken(reader.ReadInt32())
    | OperandType.InlineBrTarget ->
        let offset = int64 (reader.ReadInt32())
        BranchTarget(int (offset + GetPosition(reader) - startPos))
    | OperandType.ShortInlineBrTarget ->
        let offset = int64 (reader.ReadSByte())
        BranchTarget(int (offset + GetPosition(reader) - startPos))
    | OperandType.InlineSwitch ->
        let branches =
            [| 1..reader.ReadInt32() |]
            |> Array.map (fun _ -> reader.ReadInt32())
        let shift = int (GetPosition(reader) - startPos)
        let result = branches |> Array.map (fun t -> t + shift)
        SwitchTarget(result)
    | OperandType.InlineVar -> VarOperand(reader.ReadInt32())
    | OperandType.ShortInlineVar -> VarOperand(int (reader.ReadSByte()))
    // metadata tokens
    | OperandType.InlineField -> MetadataToken(reader.ReadInt32())
    | OperandType.InlineMethod -> MetadataToken(reader.ReadInt32())
    | OperandType.InlineSig -> MetadataToken(reader.ReadInt32())
    | OperandType.InlineTok -> MetadataToken(reader.ReadInt32())
    | OperandType.InlineType -> MetadataToken(reader.ReadInt32())
    | OperandType.InlineNone -> NoOperand
    | _ -> invalidOp "not implemented"

let readOpCode (reader : BinaryReader) =
    let i = int (reader.ReadByte())
    match i with
    | 0xfe -> longOpCodes.Value.[int (reader.ReadByte())]
    | _ -> shortOpCodes.Value.[i]

let readInstruction (reader : BinaryReader, startPos : int64) =
    let opCode = readOpCode (reader)

    let result : Instruction =
        { opCode = opCode
          code = enum (int opCode.Value)
          operand = readOperand (reader, opCode, startPos) }
    result

let readInstructions (reader : BinaryReader, codeSize : int) =
    let startPos = GetPosition(reader)

    let code =
        seq {
            let mutable offset = 0
            while offset < codeSize do
                let pos = GetPosition(reader)
                let i = readInstruction (reader, startPos)
                let size = int (GetPosition(reader) - pos)
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
        let dwordMultipleSize = (msb &&& 0xf0u) >>> 4
        assert (dwordMultipleSize = uint32 3) // the fat header is 3 dwords
        let maxStackSize = int (reader.ReadUInt16())
        let codeSize = reader.ReadInt32()
        let localSig = reader.ReadInt32()
        let flags2 : MethodBodyFlags = enum (int ((msb &&& 0x0fu) <<< 8 ||| lsb))
        let code = readInstructions (reader, codeSize)

        let sehBlocks =
            if int (flags2 &&& MethodBodyFlags.MoreSects) <> 0 then
                readSEHBlocks (reader)
            else [||]

        let result : MethodBody =
            { maxStackSize = maxStackSize
              localSig = localSig
              code = code
              sehBlocks = sehBlocks }

        result

    let readTinyMethod() =
        let codeSize = lsb >>> 2
        let code = readInstructions (reader, int codeSize)

        let result : MethodBody =
            { maxStackSize = 8
              localSig = 0
              code = code
              sehBlocks = [||] }
        result

    match format with
    | MethodBodyFlags.FatFormat -> readFatMethod()
    | MethodBodyFlags.TinyFormat -> readTinyMethod()
    | MethodBodyFlags.TinyFormat1 -> readTinyMethod()
    | _ -> invalidOp "bad method format"

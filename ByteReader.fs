module ByteReader

open System
open System.IO

// binary data reader based on free monads
module Implementation =
    type Instruction<'a> =
        | Byte of (Byte -> 'a)
        | Int16 of (Int16 -> 'a)
        | Int32 of (Int32 -> 'a)
        | Int64 of (Int64 -> 'a)
        | Blob of int * (Byte[] -> 'a)
    type Program<'a> =
        | Free of Program<'a> Instruction
        | Pure of 'a

    let rec private mapI f = function
        | Byte (next) -> Byte (next >> f)
        | Int16 (next) -> Int16 (next >> f)
        | Int32 (next) -> Int32 (next >> f)
        | Int64 (next) -> Int64 (next >> f)
        | Blob (len, next) -> Blob (len, next >> f)

    let rec bind f = function
        | Free x -> x |> mapI (bind f) |> Free
        | Pure x -> f x

    type ByteReaderBuilder () =
        member this.Bind (x, f) = bind f x
        member this.Return x = Pure x
        member this.ReturnFrom x = x
        member this.Zero () = Pure ()
        
open Implementation

let reader = ByteReaderBuilder ()

let readByte = Free (Byte Pure)
let readInt16 = Free (Int16 Pure)
let readInt32 = Free (Int32 Pure)
let readInt64 = Free (Int64 Pure)
let readBlob len = Free (Blob (len, Pure))

let createStreamReader (r: BinaryReader) =
    let rec interpret: Program<'a> -> 'a = function
        | Pure x -> x
        | Free (Byte(next)) -> r.ReadByte() |> next |> interpret
        | Free (Int16(next)) -> r.ReadInt16() |> next |> interpret
        | Free (Int32(next)) -> r.ReadInt32() |> next |> interpret
        | Free (Int64(next)) -> r.ReadInt64() |> next |> interpret
        | Free (Blob(len, next)) -> r.ReadBytes(len) |> next |> interpret
    interpret

let readData = reader {
    let! len = readInt16
    let! data = readBlob (int len)
    return data
}

type Header = {
    sign: Int32
    data: byte array
}

let readHeader = reader {
    let! sign = readInt32
    let! data = readData
    return { sign = sign; data = data }
}

let demo() =
    
    let bytes = [| 0uy; 1uy; 3uy; 5uy; 2uy; 11uy; 12uy |]
    let streamReader = createStreamReader (new BinaryReader(new MemoryStream(bytes)))

    let header = streamReader readHeader
    ()
module ByteReader

open System

// binary data reader based on free monads

type Instruction<'a> =
    | Byte of (Byte -> 'a)
    | Int16 of (Int16 -> 'a)
    | Int32 of (Int32 -> 'a)
    | Int64 of (Int64 -> 'a)
    | Blob of int * (Byte[] -> 'a)
and Program<'a> =
    | Free of Program<'a> Instruction
    | Pure of 'a

let rec private mapI f = function
    | Byte (next) -> Byte (next >> f)
    | Int16 (next) -> Int16 (next >> f)
    | Int32 (next) -> Int32 (next >> f)
    | Int64 (next) -> Int64 (next >> f)
    | Blob (len, next) -> Blob (len, next >> f)

and bind f = function
    | Free x -> x |> mapI (bind f) |> Free
    | Pure x -> f x
and mapP f = bind (f >> Pure)

type ByteReaderBuilder () =
    member this.Bind (x, f) = bind f x
    member this.Return x = Pure x
    member this.ReturnFrom x = x
    member this.Zero () = Pure ()

let reader = ByteReaderBuilder ()

let readByte = Free (Byte Pure)
let readInt16 = Free (Int16 Pure)
let readInt32 = Free (Int32 Pure)
let readInt64 = Free (Int64 Pure)
let readBlob len = Free (Blob (len, Pure))

type Header = {
    sign: Int32
    data: byte array
}

let demo() =
    
    let headerReader = reader {
        let! sign = readInt32
        let! len = readInt16

        let! data = readBlob (int len)
        return { sign = sign; data = data }
    }
    ()
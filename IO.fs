module Fint.IO

open System
open System.IO

let ReadBytes(reader : BinaryReader, size : int) =
    let buf : byte array = Array.zeroCreate (size)
    let n = reader.Read(buf, 0, size)
    assert (n = size)
    buf

let Align4(reader : BinaryReader) =
    reader.BaseStream.Position <- ((reader.BaseStream.Position + 3L) / 4L) * 4L
    ()

let Skip (reader : BinaryReader, size : int) =
    reader.BaseStream.Position <- reader.BaseStream.Position + int64 size
    ()

let ReadZeroTerminatedString(reader : BinaryReader, length : int) =
    let bytes = ReadBytes(reader, length)
    let nonZero t = int t <> 0
    let toChar t = (char) t
    bytes
    |> Seq.takeWhile nonZero
    |> Seq.map toChar
    |> List.ofSeq
    |> List.toArray
    |> String

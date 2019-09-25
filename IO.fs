module Fint.IO

open System
open System.IO
open System.Text

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

let ReadAlignedString(reader: BinaryReader, maxLength: int) =
    let mutable read = 0
    let chars() = seq {
        let mutable reading = true
        while reading && read < maxLength do
            let current = reader.ReadByte()
            reading <- current <> byte 0
            read <- read + (if reading then 1 else 0)
            if reading then yield [(char)current] |> Seq.ofList
            else yield Seq.empty<char>
    }
    let s: string = chars() |> Seq.concat |> Seq.toArray |> String
    let align = -1 + ((read + 4) &&& (~~~3)) - read
    Skip(reader, align)
    s

let ReadUTF8(reader: BinaryReader, bytesToRead: int) =
    let readBytesZ() =
        seq {
            let mutable reading = true
            while reading do
                let b: byte = reader.ReadByte()
                reading <- int b <> 0
                if reading then yield [b] |> Seq.ofList
                else yield Seq.empty<byte>
        } |> Seq.concat |> Seq.toArray
    let bytes = if bytesToRead > 0 then ReadBytes(reader, bytesToRead) else readBytesZ()
    Encoding.UTF8.GetString(bytes)
 
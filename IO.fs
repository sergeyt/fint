module Fint.IO

open System
open System.IO
open System.Text

let MakeReader(blob: byte array) =
    new BinaryReader(new MemoryStream(blob))

let GetPosition(reader : BinaryReader) =
    reader.BaseStream.Position

let Move(reader : BinaryReader, pos: int64) =
    reader.BaseStream.Position <- pos
    ()

let Restore(reader : BinaryReader, read: unit -> 'a) =
    let pos = GetPosition(reader)
    let result = read()
    Move(reader, pos)
    result

let ReadBytes(reader : BinaryReader, size : int) =
    let buf : byte array = Array.zeroCreate (size)
    let n = reader.Read(buf, 0, size)
    assert (n = size)
    buf

let Align4(reader : BinaryReader) =
    let pos = GetPosition(reader)
    Move(reader, ((pos + 3L) / 4L) * 4L)
    ()

let Skip (reader : BinaryReader, size : int) =
    Move(reader, GetPosition(reader) + int64 size)
    ()

let ReadZeroTerminatedString(reader : BinaryReader, length : int) =
    let bytes = ReadBytes(reader, length)
    bytes
    |> Seq.takeWhile (fun t -> int t <> 0)
    |> Seq.map char
    |> List.ofSeq
    |> List.toArray
    |> String

let ReadAlignedString(reader: BinaryReader, maxLength: int) =
    let mutable read = 0
    let chars() = seq {
        let mutable reading = true
        while reading && read < maxLength do
            let current = char (reader.ReadByte())
            reading <- current <> char 0
            read <- read + (if reading then 1 else 0)
            if reading then yield String(current, 1)
            else yield ""
    }
    let s: string = chars() |> Seq.toArray |> String.Concat
    let align = -1 + ((read + 4) &&& ~~~3) - read
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
 
let ReadPackedInt(reader:BinaryReader) =
    let b0 = int(reader.ReadByte())
    let read2() =
        let b1 = int(reader.ReadByte())
        let read4() =
            let b2 = int(reader.ReadByte())
            let b3 = int(reader.ReadByte())
            ((b0 &&& 0x3F) <<< 24) ||| (b1 <<< 16) ||| (b2 <<< 8) ||| b3
        if ((b0 &&& 0xC0) = 0x80) then (((b0 &&& 0x3F) <<< 8) ||| b1)
        else read4()
    if ((b0 &&& 0x80) = 0) then b0
    else read2()

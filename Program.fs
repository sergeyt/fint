module Fint.CLI

open System.IO
open Fint.PEImage

[<EntryPoint>]
let main argv =
    use input = File.OpenRead(argv.[0])
    let reader = new BinaryReader(input)
    let headers = ReadExecutableHeaders(reader)
    printfn "%A" headers
    0 // return an integer exit code

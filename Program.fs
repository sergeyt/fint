module Fint.CLI

open System.IO
open Fint.MetaReader

[<EntryPoint>]
let main argv =
    use input = File.OpenRead(argv.[0])
    let reader = new BinaryReader(input)
    let meta = MetaReader(reader)
    printfn "%A" (meta.dump())
    0 // return an integer exit code

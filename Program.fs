module Fint.CLI

open System.IO
open Fint.MetaReader

[<EntryPoint>]
let main argv =
    use input = File.OpenRead(argv.[0])
    let reader = new BinaryReader(input)
    let meta = MetaReader(reader)
    printfn "%A" (meta.dump())
    let entry = meta.readEntryPoint()
    printfn "%A" (entry.Value.body())
    0 // return an integer exit code

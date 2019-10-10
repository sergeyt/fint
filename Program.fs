module Fint.CLI

open System.IO
open Fint.Enums
open Fint.MetaReader
open Fint.Interpreter

let checkCorlib() =
    let path = (typeof<string>).Assembly.Location
    use input = File.OpenRead(path)
    let reader = new BinaryReader(input)
    let meta = MetaReader(reader)
    // printfn "%A" (meta.dump())
    let methodCount = meta.rowCount TableId.MethodDef
    let methods = [ 0 .. methodCount - 1 ] |> List.map meta.readMethod
    let body = methods |> List.map (fun m -> m.body()) |> List.filter (fun t -> t <> None)
    printfn "%A" body
    printfn "%d" body.Length
    ()

[<EntryPoint>]
let main argv =
    use input = File.OpenRead(argv.[0])
    let reader = new BinaryReader(input)
    run reader
    // let meta = MetaReader(reader)
    // printfn "%A" (meta.dump())
    // let entry = meta.readEntryPoint()
    // printfn "%A" (entry.Value.body())
    0 // return an integer exit code

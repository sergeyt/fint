module Fint.CLI

open System.IO
open Fint.Enums
open Fint.MetaReader
open Fint.Interpreter

let resolvePath path =
    match path with
    | "corlib" -> (typeof<string>).Assembly.Location
    | t -> t

let dumpMeta path =
    use input = File.OpenRead(resolvePath path)
    let reader = new BinaryReader(input)
    let meta = MetaReader(reader)
    printfn "%A" (meta.dump())
    ()

let dumpMethods path =
    use input = File.OpenRead(resolvePath path)
    let reader = new BinaryReader(input)
    let meta = MetaReader(reader)
    let methodCount = meta.rowCount TableId.MethodDef
    let methods = [ 0 .. methodCount - 1 ] |> List.map meta.readMethod
    let body = methods |> List.map (fun m -> m.body()) |> List.filter (fun t -> t <> None)
    printfn "%A" body
    printfn "%d" body.Length
    ()

[<EntryPoint>]
let main argv =
    let cmd = argv.[0]
    match cmd with
    | "meta" ->
        dumpMeta argv.[1]
        0
    | "methods" ->
        dumpMethods argv.[1]
        0
    | "run" ->
        use input = File.OpenRead(argv.[1])
        let reader = new BinaryReader(input)
        run reader |> ignore
        0
    | s ->
      printfn "unknown command %s" s
      printfn ""
      -1

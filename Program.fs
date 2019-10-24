module Fint.CLI

open System
open System.IO
open Fint.Enums
open Fint.MethodBody
open Fint.Types
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
    let withBody = methods |> List.filter (fun m -> m.body() <> None)

    let dumpCode (body: MethodBody) =
        let dumpOperand v =
            match v with
            | Int32Operand t -> t.ToString()
            | Int64Operand t -> t.ToString()
            | Float32Operand t -> t.ToString()
            | Float64Operand t -> t.ToString()
            | SwitchTarget t -> sprintf "%A" t
            | BranchTarget t -> t.ToString()
            | MetadataToken t ->
                let result = meta.resolveToken (uint32 t)
                match result with
                | StringToken s -> sprintf "0x%X=%A" t s
                | RowToken r ->
                    let table = meta.findTable r.table
                    let cells =
                        Array.zip table.columns r.cells
                        |> Array.map (fun (c, v) -> sprintf "%s=%s" c.name (meta.dumpCell v))
                    sprintf "0x%X=%A(%s)" t r.table (String.Join(";", cells))
            | _ -> ""

        let lines =
            Array.zip body.code [| 0 .. body.code.Length - 1 |]
            |> Array.map (fun (i, k) -> sprintf "%d: %A %s" k i.opCode (dumpOperand i.operand))
        String.Join("\n", lines)

    let dumpMethod (m: MethodDef) =
        match m.body() with
        | Some body ->
            let sep = new String('-', m.name.Length)
            sprintf "%s\n%s\n%s\n%s" sep m.name sep (dumpCode body)
        | None -> failwith "expect method body"

    printfn "methods with body: %d" withBody.Length
    for m in withBody do
        printfn "%s" (dumpMethod (m))
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

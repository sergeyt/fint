module Fint.Interpreter

open System
open Fint.Collections
open Fint.Enums
open Fint.MethodBody
open Fint.Meta
open Fint.MetaReader

type CallContext =
    { method: MethodDef
      ip: int
      stack: ImmutableStack<obj>
      result: obj
      callStack: ImmutableStack<CallContext> }

let expectBody method =
    match method.body() with
    | None -> failwith "no method body"
    | Some body -> body

let run reader =
    let meta = MetaReader(reader)
    let entry = meta.readEntryPoint()

    let mutable callStack = Empty

    let pushCall method =
        let ctx: CallContext =
            { method = method
              ip = 0
              stack = Empty
              result = null
              callStack = callStack }
        callStack <- callStack.Push ctx
        { ctx with callStack = callStack }

    let next ctx = { ctx with ip = ctx.ip + 1 }
    let push ctx value = next { ctx with stack = ctx.stack.Push value }

    let pop ctx =
        let v = ctx.stack.Pop()
        (v, next { ctx with stack = ctx.stack.Top() })
    let ret ctx =
        match ctx.stack.IEmpty with
        | true -> next ctx
        | false ->
            let (v, c) = pop ctx
            next { c with result = v }

    let resolveString token =
        let t = meta.resolveToken token
        match t with
        | StringToken s -> s
        | RowToken _ -> failwith "expect string token"

    let resolveRow token =
        let t = meta.resolveToken token
        match t with
        | RowToken r -> r
        | StringToken _ -> failwith "expect metadata row"

    let dataInt32 v =
        match v with
        | Int32Operand t -> t
        | Int64Operand t -> int t
        | _ -> failwith "expect int32 operand"

    let dataInt64 v =
        match v with
        | Int32Operand t -> int64 t
        | Int64Operand t -> t
        | _ -> failwith "expect int64 operand"

    let dataFloat32 v =
        match v with
        | Float32Operand t -> t
        | _ -> failwith "expect float32 operand"

    let dataFloat64 v =
        match v with
        | Float64Operand t -> t
        | _ -> failwith "expect float64 operand"

    let dataString v =
        match v with
        | MetadataToken t -> resolveString (uint32 t)
        | _ -> failwith "expect float64 operand"

    let dataToken v =
        match v with
        | MetadataToken t -> uint32 t
        | _ -> failwith "expect metadata token operand"

    let callMethod ctx method =
        // TODO pop params
        ctx

    let isConsole parent =
        match parent with
        | TypeRefParent p -> p.ns = "System" && p.name = "Console"
        | _ -> false

    let callConsole ctx memberRef =
        let (v, c) = pop ctx
        match memberRef.name with
        | "WriteLine" -> Console.WriteLine(v)
        | "Write" -> Console.Write(v)
        | _ -> failwith "not implemented"
        next c

    let callMemberRef ctx memberRef =
        // TODO resolve method and call it
        match isConsole memberRef.parent with
        | false -> failwith "only console is supported for now"
        | true -> callConsole ctx memberRef

    let call ctx token =
        let row = resolveRow token
        match row.table with
        | TableId.MethodDef -> callMethod ctx (meta.makeMethod row.cells)
        | TableId.MemberRef -> callMemberRef ctx (meta.makeMemberRef row.cells)
        | _ -> failwith "expect MethodDef or MemberRef"

    let op ctx =
        let body = expectBody ctx.method
        let i = body.code.[ctx.ip]
        match i.code with
        | InstructionCode.Nop -> next ctx
        // constants
        | InstructionCode.Ldnull -> push ctx null
        | InstructionCode.Ldc_I4_M1 -> push ctx -1
        | InstructionCode.Ldc_I4_0 -> push ctx 0
        | InstructionCode.Ldc_I4_1 -> push ctx 1
        | InstructionCode.Ldc_I4_2 -> push ctx 2
        | InstructionCode.Ldc_I4_3 -> push ctx 3
        | InstructionCode.Ldc_I4_4 -> push ctx 4
        | InstructionCode.Ldc_I4_5 -> push ctx 5
        | InstructionCode.Ldc_I4_6 -> push ctx 6
        | InstructionCode.Ldc_I4_7 -> push ctx 7
        | InstructionCode.Ldc_I4_8 -> push ctx 8
        | InstructionCode.Ldc_I4_S -> push ctx (dataInt32 i.operand)
        | InstructionCode.Ldc_I4 -> push ctx (dataInt32 i.operand)
        | InstructionCode.Ldc_I8 -> push ctx (dataInt64 i.operand)
        | InstructionCode.Ldc_R4 -> push ctx (dataFloat32 i.operand)
        | InstructionCode.Ldc_R8 -> push ctx (dataFloat64 i.operand)
        | InstructionCode.Ldstr -> push ctx (dataString i.operand)
        // stack operations
        | InstructionCode.Dup -> next { ctx with stack = ctx.stack.Push(ctx.stack.Top()) }
        | InstructionCode.Pop -> next { ctx with stack = ctx.stack.Top() }
        | InstructionCode.Ret -> ret ctx
        // call
        | InstructionCode.Call -> call ctx (dataToken i.operand)
        | _ -> failwith "not implemented"

    let eval ctx =
        let body = expectBody ctx.method
        let mutable c = ctx
        while c.ip < body.code.Length do
            c <- op c

    let start main =
        let ctx = pushCall main
        eval ctx
        0

    match entry with
    | None -> failwith "no entry point"
    | Some m -> start m

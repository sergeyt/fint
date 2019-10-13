module Fint.Interpreter

open System
open Fint.Collections
open Fint.Enums
open Fint.MethodBody
open Fint.Meta
open Fint.MetaReader
open Fint.Signature
open Fint.Variant
open Fint.Utils

type CallContext =
    { method: MethodDef
      ip: int
      stack: ImmutableStack<Variant>
      vars: Variant array
      args: Variant array
      result: Variant
      callStack: ImmutableStack<CallContext> }

let expectBody method =
    match method.body() with
    | None -> failwith "no method body"
    | Some body -> body

let run reader =
    let meta = MetaReader(reader)
    let entry = meta.readEntryPoint()

    let mutable callStack = Empty
    
    let allocPrimitive t =
        match t with
        | ElementType.Boolean -> Variant(VarBool(false))
        | ElementType.Char -> Variant(VarChar(char 0))
        | ElementType.Int8 -> Variant(VarInt8(sbyte 0))
        | ElementType.UInt8 -> Variant(VarUInt8(byte 0))
        | ElementType.Int16 -> Variant(VarInt16(int16 0))
        | ElementType.UInt16 -> Variant(VarUInt16(uint16 0))
        | ElementType.Int32 -> Variant(VarInt32(0))
        | ElementType.UInt32 -> Variant(VarUInt32(0u))
        | ElementType.Int64 -> Variant(VarInt64(0L))
        | ElementType.UInt64 -> Variant(VarInt32(0))
        | ElementType.Single -> Variant(VarSingle(float32 0))
        | ElementType.Double -> Variant(VarDouble(0.0))
        | ElementType.String -> Variant(VarObject(null))
        | ElementType.Object -> Variant(VarObject(null))
        | _ -> notSupported()

    let allocType t =
        match t with
        | PrimitiveTypeSig t -> allocPrimitive t
        | _ -> failwith "not implemented"

    let allocVars vars = vars |> Array.map (fun v -> allocType v.Type)

    let makeCall method args =
        let vars = allocVars (method.localVars())
        let ctx: CallContext =
            { method = method
              ip = 0
              stack = Empty
              vars = vars
              args = args
              result = Variant(VarNull)
              callStack = callStack }
        callStack <- callStack.Push ctx
        { ctx with callStack = callStack }

    let rec eval ctx =
        let goto ctx i = { ctx with ip = i }
        let next ctx = goto ctx (ctx.ip + 1)
        let push ctx value = next { ctx with stack = ctx.stack.Push value }
        let popval ctx = 
            let v = ctx.stack.Pop()
            (v, { ctx with stack = ctx.stack.Top() })
        let pop ctx =
            let v = ctx.stack.Pop()
            (v, next { ctx with stack = ctx.stack.Top() })
        let ret ctx =
            match ctx.stack.IEmpty with
            | true -> next ctx
            | false ->
                let (v, c) = pop ctx
                next { c with result = v }
        let ldloc ctx i = push ctx ctx.vars.[i]
        let ldarg ctx i = push ctx ctx.args.[i]
        let stloc ctx i =
            let (v, c) = pop ctx
            c.vars.[i] <- v
            c
        let starg ctx i =
            let (v, c) = pop ctx
            c.args.[i] <- v
            c

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
            | BranchTarget t -> t
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

        let popArgs ctx signature =
            let args: Variant array = Array.zeroCreate signature.Params.Length
            let mutable c = ctx
            let mutable i = args.Length - 1
            while i >= 0 do
                let (v, t) = popval c
                c <- t
                args.[i] <- v
                i <- i - 1
            (args, c)

        let callMethod ctx (method: MethodDef) =
            let (args, c) = popArgs ctx method.signature
            if not (IsStaticMethod method)
            then failwith "instance call is not implemented"
            let mc = eval (makeCall method args)
            callStack <- callStack.Top()
            match IsVoidMethod method with
                | true -> next c
                | false -> push c mc.result

        let typeRefName parent =
            match parent with
            | TypeRefParent p -> p.ns + "." + p.name
            | _ -> failwith "not supported"

        let callConsole ctx memberRef =
            let (v, c) = pop ctx
            match memberRef.name with
            | "WriteLine" -> Console.WriteLine(v.ToObject())
            | "Write" -> Console.Write(v.ToObject())
            | _ -> failwith "not implemented"
            next c

        let callString ctx memberRef =
            let (args, c) = popArgs ctx memberRef.signature
            match memberRef.name with
            | "Concat" ->
                let s = String.Concat(args |> Array.map (fun v -> v.ToString()))
                push c (Variant(VarString(s)))
            | _ -> failwith "not implemented"

        let callMemberRef ctx memberRef =
            // TODO resolve method and call it
            match typeRefName memberRef.parent with
            | "System.Console" -> callConsole ctx memberRef
            | "System.String" -> callString ctx memberRef
            | name -> failwith (sprintf "call of %s.%s is not implemented" name memberRef.name)

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
            | InstructionCode.Ldnull -> push ctx (Variant(VarNull))
            | InstructionCode.Ldc_I4_M1 -> push ctx (Variant(VarInt32(-1)))
            | InstructionCode.Ldc_I4_0 -> push ctx (Variant(VarInt32(0)))
            | InstructionCode.Ldc_I4_1 -> push ctx (Variant(VarInt32(1)))
            | InstructionCode.Ldc_I4_2 -> push ctx (Variant(VarInt32(2)))
            | InstructionCode.Ldc_I4_3 -> push ctx (Variant(VarInt32(3)))
            | InstructionCode.Ldc_I4_4 -> push ctx (Variant(VarInt32(4)))
            | InstructionCode.Ldc_I4_5 -> push ctx (Variant(VarInt32(5)))
            | InstructionCode.Ldc_I4_6 -> push ctx (Variant(VarInt32(6)))
            | InstructionCode.Ldc_I4_7 -> push ctx (Variant(VarInt32(7)))
            | InstructionCode.Ldc_I4_8 -> push ctx (Variant(VarInt32(8)))
            | InstructionCode.Ldc_I4_S -> push ctx (Variant(VarInt32(dataInt32 i.operand)))
            | InstructionCode.Ldc_I4 -> push ctx (Variant(VarInt32(dataInt32 i.operand)))
            | InstructionCode.Ldc_I8 -> push ctx (Variant(VarInt64((dataInt64 i.operand))))
            | InstructionCode.Ldc_R4 -> push ctx (Variant(VarSingle(dataFloat32 i.operand)))
            | InstructionCode.Ldc_R8 -> push ctx (Variant(VarDouble(dataFloat64 i.operand)))
            | InstructionCode.Ldstr -> push ctx (Variant(VarString(dataString i.operand)))
            // load instructions
            | InstructionCode.Ldloc_0 -> ldloc ctx 0
            | InstructionCode.Ldloc_1 -> ldloc ctx 1
            | InstructionCode.Ldloc_2 -> ldloc ctx 2
            | InstructionCode.Ldloc_3 -> ldloc ctx 3
            | InstructionCode.Ldloc_S -> ldloc ctx (dataInt32 i.operand)
            | InstructionCode.Ldarg_0 -> ldarg ctx 0
            | InstructionCode.Ldarg_1 -> ldarg ctx 1
            | InstructionCode.Ldarg_2 -> ldarg ctx 2
            | InstructionCode.Ldarg_3 -> ldarg ctx 3
            | InstructionCode.Ldarg_S
            | InstructionCode.Ldarg -> ldarg ctx (dataInt32 i.operand)
            // store instructions
            | InstructionCode.Stloc_0 -> stloc ctx 0
            | InstructionCode.Stloc_1 -> stloc ctx 1
            | InstructionCode.Stloc_2 -> stloc ctx 2
            | InstructionCode.Stloc_3 -> stloc ctx 3
            | InstructionCode.Stloc_S
            | InstructionCode.Stloc -> stloc ctx (dataInt32 i.operand)
            | InstructionCode.Starg_S
            | InstructionCode.Starg -> starg ctx (dataInt32 i.operand)
            // stack operations
            | InstructionCode.Dup -> next { ctx with stack = ctx.stack.Push(ctx.stack.Pop()) }
            | InstructionCode.Pop -> next { ctx with stack = ctx.stack.Top() }
            | InstructionCode.Ret -> ret ctx
            // call instructions
            | InstructionCode.Call -> call ctx (dataToken i.operand)
            // branching instructions
            | InstructionCode.Br_S
            | InstructionCode.Br -> goto ctx (dataInt32 i.operand)
            | _ -> failwith "not implemented"

        let body = expectBody ctx.method
        let mutable c = ctx
        while c.ip < body.code.Length do
            c <- op c
        c

    let start main =
        let ctx = makeCall main [||]
        eval ctx |> ignore
        0

    match entry with
    | None -> failwith "no entry point"
    | Some m -> start m

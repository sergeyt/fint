module Fint.Interpreter

open System
open Fint.Collections
open Fint.Enums
open Fint.MethodBody
open Fint.Types
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

    let convTypeSig (v: Variant) t =
        match t with
        | PrimitiveTypeSig t ->
            match t with
                | ElementType.Boolean -> v.ChangeType(TypeCode.Boolean)
                | ElementType.Char -> v.ChangeType(TypeCode.Char)
                | ElementType.Int8 -> v.ChangeType(TypeCode.SByte)
                | ElementType.UInt8 -> v.ChangeType(TypeCode.Byte)
                | ElementType.Int16 -> v.ChangeType(TypeCode.Int16)
                | ElementType.UInt16 -> v.ChangeType(TypeCode.UInt16)
                | ElementType.Int32 -> v.ChangeType(TypeCode.Int32)
                | ElementType.UInt32 -> v.ChangeType(TypeCode.UInt32)
                | ElementType.Int64 -> v.ChangeType(TypeCode.Int64)
                | ElementType.UInt64 -> v.ChangeType(TypeCode.UInt64)
                | ElementType.Single -> v.ChangeType(TypeCode.Single)
                | ElementType.Double -> v.ChangeType(TypeCode.Double)
                | ElementType.String -> v.ChangeType(TypeCode.String)
                | _ -> v
        | _ -> v

    let expectRowToken t =
        match t with
        | RowToken x -> x
        | _ -> failwith "expect metadata row"

    let convByToken (v: Variant) (row: Row) =
        match row.table with
        | TableId.TypeRef ->
            let t = meta.makeTypeRef(row.cells)
            let name = t.ns + "." + t.name
            match name with
            | "System.Boolean" -> v.ChangeType(TypeCode.Boolean)
            | "System.Char" -> v.ChangeType(TypeCode.Char)
            | "System.SByte" -> v.ChangeType(TypeCode.SByte)
            | "System.Byte" -> v.ChangeType(TypeCode.Byte)
            | "System.Int16" -> v.ChangeType(TypeCode.Int16)
            | "System.UInt16" -> v.ChangeType(TypeCode.UInt16)
            | "System.Int32" -> v.ChangeType(TypeCode.Int32)
            | "System.UInt32" -> v.ChangeType(TypeCode.UInt32)
            | "System.Int64" -> v.ChangeType(TypeCode.Int64)
            | "System.UInt64" -> v.ChangeType(TypeCode.UInt64)
            | "System.Single" -> v.ChangeType(TypeCode.Single)
            | "System.Decimal" -> v.ChangeType(TypeCode.Decimal)
            | "System.String" -> v.ChangeType(TypeCode.String)
            | _ -> v
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
        let body = expectBody ctx.method

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

        let isBranch ctx op =
            match op with 
            | BranchOp.False -> 
                let (v, c) = popval ctx
                (not (v.ToBoolean()), c)
            | BranchOp.True -> 
                let (v, c) = popval ctx 
                (v.ToBoolean(), c)
            | BranchOp.Null ->
                let (v, c) = popval ctx 
                (v.IsNull(), c)
            | BranchOp.NotNull ->
                let (v, c) = popval ctx 
                (not (v.IsNull()), c)
            | BranchOp.Equal ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x = y, cx)
            | BranchOp.NotEqual ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x <> y, cx)
            | BranchOp.NotEqualUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x.ToUnsigned() <> y.ToUnsigned(), cx)
            | BranchOp.LessThan ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x < y, cx)
            | BranchOp.LessThanUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x.ToUnsigned() < y.ToUnsigned(), cx)
            | BranchOp.LessThanOrEqual ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x <= y, cx)
            | BranchOp.LessThanOrEqualUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x.ToUnsigned() <= y.ToUnsigned(), cx)
            | BranchOp.GreaterThan ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x > y, cx)
            | BranchOp.GreaterThanUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x.ToUnsigned() > y.ToUnsigned(), cx)
            | BranchOp.GreaterThanOrEqual ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x >= y, cx)
            | BranchOp.GreaterThanOrEqualUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                (x.ToUnsigned() >= y.ToUnsigned(), cx)
            | _ -> failwith "not implemented"

        let branch ctx op i =
            let (v, c) = isBranch ctx op
            match v with
            | true -> goto c i
            | false -> next c

        let calc ctx op ovf =
            match op with
            | CalcOp.Add ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x + y)
            | CalcOp.AddUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x.ToUnsigned() + y.ToUnsigned())
            | CalcOp.Sub ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x - y)
            | CalcOp.SubUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x.ToUnsigned() - y.ToUnsigned())
            | CalcOp.Mul ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x * y)
            | CalcOp.MulUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x.ToUnsigned() * y.ToUnsigned())
            | CalcOp.Div ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x / y)
            | CalcOp.DivUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x.ToUnsigned() / y.ToUnsigned())
            | CalcOp.Rem ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x % y)
            | CalcOp.RemUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x.ToUnsigned() % y.ToUnsigned())
            | CalcOp.BitwiseNot ->
                let (v, c) = popval ctx 
                push c (Variant.op_BitwiseNot(v))
            | CalcOp.BitwiseAnd ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x &&& y)
            | CalcOp.BitwiseOr ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x ||| y)
            | CalcOp.Xor ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x ^^^ y)
            | CalcOp.ShiftLeft ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x <<< (y.ToInt32()))
            | CalcOp.ShiftRight ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x >>> (y.ToInt32()))
            | CalcOp.ShiftRightUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (x.ToUnsigned() >>> (y.ToInt32()))
            | CalcOp.Neg ->
                let (v, c) = popval ctx 
                push c (Variant.op_Negate(v))
            | CalcOp.Not ->
                let (v, c) = popval ctx 
                push c (Variant.op_Not(v))
            | CalcOp.Equal ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (Variant(VarBool(x = y)))
            | CalcOp.LessThan ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (Variant(VarBool(x < y)))
            | CalcOp.LessThanUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (Variant(VarBool(x.ToUnsigned() < y.ToUnsigned())))
            | CalcOp.GreaterThan ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (Variant(VarBool(x > y)))
            | CalcOp.GreaterThanUnsigned ->
                let (y, cy) = popval ctx 
                let (x, cx) = popval cy
                push cx (Variant(VarBool(x.ToUnsigned() > y.ToUnsigned())))
            | _ -> failwith "not implemented"

        let conv ctx t ovf =
            let (v, c) = popval ctx
            push c (v.ChangeType(t))
        let convun ctx t ovf =
            let (v, c) = popval ctx
            // TODO check overflow
            push c (v.ToUnsigned().ChangeType(t))

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
                args.[i] <- convTypeSig v (signature.Params.[i])
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
            let (a, c) = popArgs ctx memberRef.signature
            let args = a |> Array.map (fun t -> t.ToObject())
            match memberRef.name with
            | "WriteLine" ->    
                match a.Length with
                | 1 -> Console.WriteLine(args.[0])
                | _ -> Console.WriteLine(a.[0].ToString(), args.[1..])
            | "Write" ->
                match a.Length with
                | 1 -> Console.Write(args.[0])
                | _ -> Console.Write(a.[0].ToString(), args.[1..])
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
            | InstructionCode.Ldc_I4_S
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
            | InstructionCode.Brfalse_S
            | InstructionCode.Brfalse -> branch ctx BranchOp.False (dataInt32 i.operand)
            | InstructionCode.Brtrue_S
            | InstructionCode.Brtrue -> branch ctx BranchOp.True (dataInt32 i.operand)
            | InstructionCode.Beq_S
            | InstructionCode.Beq -> branch ctx BranchOp.Equal (dataInt32 i.operand)
            | InstructionCode.Bge_S
            | InstructionCode.Bge -> branch ctx BranchOp.GreaterThanOrEqual (dataInt32 i.operand)
            | InstructionCode.Bgt_S
            | InstructionCode.Bgt -> branch ctx BranchOp.GreaterThan (dataInt32 i.operand)
            | InstructionCode.Ble_S
            | InstructionCode.Ble -> branch ctx BranchOp.LessThanOrEqual (dataInt32 i.operand)
            | InstructionCode.Blt_S
            | InstructionCode.Blt -> branch ctx BranchOp.LessThan (dataInt32 i.operand)
            | InstructionCode.Bne_Un_S
            | InstructionCode.Bne_Un -> branch ctx BranchOp.NotEqualUnsigned (dataInt32 i.operand)
            | InstructionCode.Bge_Un_S
            | InstructionCode.Bge_Un -> branch ctx BranchOp.GreaterThanOrEqualUnsigned (dataInt32 i.operand)
            | InstructionCode.Bgt_Un_S
            | InstructionCode.Bgt_Un -> branch ctx BranchOp.GreaterThanOrEqualUnsigned (dataInt32 i.operand)
            | InstructionCode.Ble_Un_S
            | InstructionCode.Ble_Un -> branch ctx BranchOp.LessThanOrEqualUnsigned (dataInt32 i.operand)
            | InstructionCode.Blt_Un_S
            | InstructionCode.Blt_Un -> branch ctx BranchOp.LessThanUnsigned (dataInt32 i.operand)
            | InstructionCode.Switch ->
                match i.operand with
                | SwitchTarget targets ->
                    let (v, c) = popval ctx
                    let index = v.ToInt32()
                    if index >= 0 && index < targets.Length
                    then goto c targets.[index]
                    else next c
                | _ -> invalidOp "expect switch operand"
            // arithmetic operations
            | InstructionCode.Add -> calc ctx CalcOp.Add false
            | InstructionCode.Add_Ovf -> calc ctx CalcOp.Add true
            | InstructionCode.Add_Ovf_Un -> calc ctx CalcOp.AddUnsigned true
            | InstructionCode.Sub -> calc ctx CalcOp.Sub false
            | InstructionCode.Sub_Ovf -> calc ctx CalcOp.Sub true
            | InstructionCode.Sub_Ovf_Un -> calc ctx CalcOp.SubUnsigned true
            | InstructionCode.Mul -> calc ctx CalcOp.Mul false
            | InstructionCode.Mul_Ovf -> calc ctx CalcOp.Mul true
            | InstructionCode.Mul_Ovf_Un -> calc ctx CalcOp.MulUnsigned true
            | InstructionCode.Div -> calc ctx CalcOp.Div false
            | InstructionCode.Div_Un -> calc ctx CalcOp.DivUnsigned false
            | InstructionCode.Rem -> calc ctx CalcOp.Rem false
            | InstructionCode.Rem_Un -> calc ctx CalcOp.RemUnsigned false
            | InstructionCode.And -> calc ctx CalcOp.BitwiseAnd false
            | InstructionCode.Or -> calc ctx CalcOp.BitwiseOr false
            | InstructionCode.Xor -> calc ctx CalcOp.Xor false
            | InstructionCode.Shl -> calc ctx CalcOp.ShiftLeft false
            | InstructionCode.Shr -> calc ctx CalcOp.ShiftRight false
            | InstructionCode.Shr_Un -> calc ctx CalcOp.ShiftRightUnsigned false
            | InstructionCode.Neg -> calc ctx CalcOp.Neg false
            | InstructionCode.Not -> calc ctx CalcOp.BitwiseNot false
            | InstructionCode.Ceq -> calc ctx CalcOp.Equal false
            | InstructionCode.Cgt -> calc ctx CalcOp.GreaterThan false
            | InstructionCode.Cgt_Un -> calc ctx CalcOp.GreaterThanUnsigned false
            | InstructionCode.Clt -> calc ctx CalcOp.LessThan false
            | InstructionCode.Clt_Un -> calc ctx CalcOp.LessThanUnsigned false
            | InstructionCode.Box ->
                let t = meta.resolveToken (dataToken i.operand)
                let (v, c) = popval ctx
                let a = convByToken v (expectRowToken t)
                push c (Variant(VarObject(a.ToObject())))
            // conversion instructions
            | InstructionCode.Conv_I1 -> conv ctx TypeCode.SByte false
            | InstructionCode.Conv_I2 -> conv ctx TypeCode.Int16 false
            | InstructionCode.Conv_I4 -> conv ctx TypeCode.Int32 false
            | InstructionCode.Conv_I8 -> conv ctx TypeCode.Int64 false
            | InstructionCode.Conv_R4 -> conv ctx TypeCode.Single false
            | InstructionCode.Conv_R8 -> conv ctx TypeCode.Double false
            | InstructionCode.Conv_U1 -> conv ctx TypeCode.Byte false
            | InstructionCode.Conv_U2 -> conv ctx TypeCode.UInt16 false
            | InstructionCode.Conv_U4 -> conv ctx TypeCode.UInt32 false
            | InstructionCode.Conv_U8 -> conv ctx TypeCode.UInt64 false
            | InstructionCode.Conv_Ovf_I1_Un -> convun ctx TypeCode.SByte true
            | InstructionCode.Conv_Ovf_I2_Un -> convun ctx TypeCode.Int16 true
            | InstructionCode.Conv_Ovf_I4_Un -> convun ctx TypeCode.Int32 true
            | InstructionCode.Conv_Ovf_I8_Un -> convun ctx TypeCode.Int64 true
            | InstructionCode.Conv_Ovf_U1_Un -> convun ctx TypeCode.Byte true
            | InstructionCode.Conv_Ovf_U2_Un -> convun ctx TypeCode.UInt16 true
            | InstructionCode.Conv_Ovf_U4_Un -> convun ctx TypeCode.UInt32 true
            | InstructionCode.Conv_Ovf_U8_Un -> convun ctx TypeCode.UInt64 true
            | InstructionCode.Conv_Ovf_I_Un -> convun ctx TypeCode.Int32 true
            | InstructionCode.Conv_Ovf_U_Un -> convun ctx TypeCode.UInt32 true
            | InstructionCode.Conv_Ovf_I1 -> conv ctx TypeCode.SByte true
            | InstructionCode.Conv_Ovf_U1 -> conv ctx TypeCode.Byte true
            | InstructionCode.Conv_Ovf_I2 -> conv ctx TypeCode.Int16 true
            | InstructionCode.Conv_Ovf_U2 -> conv ctx TypeCode.UInt16 true
            | InstructionCode.Conv_Ovf_I4 -> conv ctx TypeCode.Int32 true
            | InstructionCode.Conv_Ovf_U4 -> conv ctx TypeCode.UInt32 true
            | InstructionCode.Conv_Ovf_I8 -> conv ctx TypeCode.Int64 true
            | InstructionCode.Conv_Ovf_U8 -> conv ctx TypeCode.UInt64 true
            | InstructionCode.Conv_I -> conv ctx TypeCode.Int32 false
            | InstructionCode.Conv_Ovf_I -> conv ctx TypeCode.Int32 true
            | InstructionCode.Conv_U -> conv ctx TypeCode.UInt32 false
            | InstructionCode.Conv_Ovf_U -> conv ctx TypeCode.UInt32 true
            | InstructionCode.Conv_R_Un -> convun ctx TypeCode.Single false
            | _ -> failwith "not implemented"

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

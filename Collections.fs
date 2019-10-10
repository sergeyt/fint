module Fint.Collections

type ImmutableStack<'T> =
    | Empty 
    | Stack of 'T * ImmutableStack<'T>

    member s.Push x = Stack(x, s)

    member s.Pop() = 
      match s with
      | Empty -> failwith "underflow"
      | Stack(t,_) -> t

    member s.Top() = 
      match s with
      | Empty -> failwith "no elements"
      | Stack(_,st) -> st

    member s.IEmpty = 
      match s with
      | Empty -> true
      | _ -> false

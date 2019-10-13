module Fint.Utils

open System
open System.Collections.Generic

let memoize fn =
    let cache = new Dictionary<_, _>()
    (fun x ->
    match cache.TryGetValue x with
    | true, v -> v
    | false, _ ->
        let v = fn (x)
        cache.Add(x, v)
        v)

let notSupported() = raise <| NotSupportedException()

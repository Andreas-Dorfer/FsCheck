﻿#light

namespace FsCheck

[<AutoOpen>]
module public Functions

open Microsoft.FSharp.Text.StructuredFormat
open Microsoft.FSharp.Text.StructuredFormat.LayoutOps

type Function<'a,'b> = 
    Function of ref<list<('a*'b)>> * ('a ->'b) with
    member x.Value = match x with Function (_,f) -> f
    member x.Table = match x with Function (table,_) -> !table //so order is correct
    interface IFormattable with
        member x.GetLayout env =
            let layoutTuple (x,y) = objL (box x) ++ sepL "->" ++ objL (box y)
            x.Table 
            |> Seq.distinct_by fst 
            |> Seq.sort_by fst 
            |> Seq.map (layoutTuple) 
            |> Seq.to_list
            |> semiListL |> braceL
        
        
let toFunction f = 
    let table = ref []
    Function (table,fun x -> let y = f x in table := (x,y)::(!table); y)



    
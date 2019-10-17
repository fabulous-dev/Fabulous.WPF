namespace Fabulous.WPF.Generator

module Helpers =
    type NullableBuilder() =
        member this.Return(x) = x
        member this.Bind(x, f) =
            match x with
            | null -> None
            | _ -> f x
       
    let nullable = NullableBuilder()

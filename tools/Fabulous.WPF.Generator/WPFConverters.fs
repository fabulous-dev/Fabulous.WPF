namespace Fabulous.WPF.Generator

open Fabulous.CodeGen.AssemblyReader
open System
open System.Windows

module WPFConverters =
    let rec tryGetStringRepresentationOfDefaultValue (defaultValue: obj) : string option =
        match defaultValue with
        // To include in Fabulous directly
        | :? Nullable<bool> as nullableBool when not nullableBool.HasValue -> Some "null"
        | :? Nullable<bool> as nullableBool when not nullableBool.Value = true -> Some "System.Nullable<bool>(true)"
        | :? Nullable<bool> as nullableBool when not nullableBool.Value = false -> Some "System.Nullable<bool>(false)"

        // WPF
        | :? Thickness as thickness when thickness = Unchecked.defaultof<Thickness> -> Some "System.Windows.Thickness(0.)"
        | _ -> Converters.tryGetStringRepresentationOfDefaultValue defaultValue
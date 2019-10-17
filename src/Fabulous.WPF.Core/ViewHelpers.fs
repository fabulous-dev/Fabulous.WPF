namespace Fabulous.WPF

open Fabulous

[<AutoOpen>]
module ViewHelpers =
    /// Checks whether two objects are reference-equal
    let identical (x: 'T) (y:'T) = System.Object.ReferenceEquals(x, y)

    let rec canReuseView (prevChild: ViewElement) (newChild: ViewElement) =
        prevChild.TargetType = newChild.TargetType

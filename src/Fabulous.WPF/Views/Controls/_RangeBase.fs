namespace Fabulous.WPF

open System.Windows.Controls.Primitives
open Fabulous

type IRangeBase =
    inherit IControl

module RangeBaseUpdaters =
    let updateSliderMinMax _ (newValueOpt: struct (float * float) voption) (node: IViewNode) =
        let rangeBase = node.Target :?> RangeBase

        match newValueOpt with
        | ValueNone ->
            rangeBase.ClearValue(RangeBase.MinimumProperty)
            rangeBase.ClearValue(RangeBase.MaximumProperty)
        | ValueSome (min, max) ->
            let currMax =
                rangeBase.GetValue(RangeBase.MaximumProperty) :?> float

            if min > currMax then
                rangeBase.SetValue(RangeBase.MaximumProperty, max)
                rangeBase.SetValue(RangeBase.MinimumProperty, min)
            else
                rangeBase.SetValue(RangeBase.MinimumProperty, min)
                rangeBase.SetValue(RangeBase.MaximumProperty, max)

module RangeBase =
    let MinimumMaximum =
        Attributes.defineSimpleScalarWithEquality<struct (float * float)>
            "RangeBase_MinimumMaximum"
            RangeBaseUpdaters.updateSliderMinMax

    let ValueWithEvent =
        Attributes.defineDependencyWithRoutedPropertyChangedEvent
            "RangeBase_ValueWithEvent"
            RangeBase.ValueProperty
            (fun target -> (target :?> RangeBase).ValueChanged)
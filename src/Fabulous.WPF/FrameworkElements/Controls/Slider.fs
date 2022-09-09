namespace Fabulous.WPF

open System.Windows.Controls
open Fabulous

type ISlider =
    inherit IRangeBase

module Slider =
    let WidgetKey = Widgets.register<Slider>()

[<AutoOpen>]
module SliderBuilders =
    type Fabulous.WPF.View with
        static member inline Slider<'msg>(min: float, max: float, value: float, onValueChanged: float -> 'msg) =
            WidgetBuilder<'msg, ISlider>(
                Slider.WidgetKey,
                RangeBase.MinimumMaximum.WithValue(struct(min, max)),
                RangeBase.ValueWithEvent.WithValue(ValueEventData.create value (fun args -> onValueChanged args.NewValue |> box))
            )
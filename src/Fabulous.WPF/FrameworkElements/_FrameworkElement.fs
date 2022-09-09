namespace Fabulous.WPF

open System.Windows
open System.Runtime.CompilerServices
open Fabulous

type IFrameworkElement =
    interface
    end

module FrameworkElement =
    let Margin = Attributes.defineDependencyWithEquality<Thickness> FrameworkElement.MarginProperty

    let HorizontalAlignment = Attributes.defineDependencyWithEquality<HorizontalAlignment> FrameworkElement.HorizontalAlignmentProperty

    let VerticalAlignment = Attributes.defineDependencyWithEquality<VerticalAlignment> FrameworkElement.VerticalAlignmentProperty

[<Extension>]
type FrameworkElementModifiers =
    [<Extension>]
    static member inline margin(this: WidgetBuilder<'msg, #IFrameworkElement>, value: Thickness) =
        this.AddScalar(FrameworkElement.Margin.WithValue(value))

    [<Extension>]
    static member inline horizontalAlignment(this: WidgetBuilder<'msg, #IFrameworkElement>, value: HorizontalAlignment) =
        this.AddScalar(FrameworkElement.HorizontalAlignment.WithValue(value))

    [<Extension>]
    static member inline verticalAlignment(this: WidgetBuilder<'msg, #IFrameworkElement>, value: VerticalAlignment) =
        this.AddScalar(FrameworkElement.VerticalAlignment.WithValue(value))

[<Extension>]
type FrameworkElementExtraModifiers =
    [<Extension>]
    static member inline margin(this: WidgetBuilder<'msg, #IFrameworkElement>, uniformLength: float) =
        this.margin(Thickness(uniformLength))

    [<Extension>]
    static member inline margin(this: WidgetBuilder<'msg, #IFrameworkElement>, left: float, top: float, right: float, bottom: float) =
        this.margin(Thickness(left, top, right, bottom))

    [<Extension>]
    static member inline centerHorizontal(this: WidgetBuilder<'msg, #IFrameworkElement>) =
        this.horizontalAlignment(HorizontalAlignment.Center)

    [<Extension>]
    static member inline centerVertical(this: WidgetBuilder<'msg, #IFrameworkElement>) =
        this.verticalAlignment(VerticalAlignment.Center)
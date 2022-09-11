namespace Fabulous.WPF

open System.Windows.Media
open System.Runtime.CompilerServices
open Fabulous

type IControl =
    inherit IFrameworkElement

module Control =
    let Background = Attributes.defineDependencySolidBrush System.Windows.Controls.Control.BackgroundProperty

    let Foreground = Attributes.defineDependencySolidBrush System.Windows.Controls.Control.ForegroundProperty

[<Extension>]
type ControlModifiers =
    [<Extension>]
    static member inline background(this: WidgetBuilder<'msg, #IFrameworkElement>, value: SolidColorBrush) =
        this.AddScalar(Control.Background.WithValue(value))

    [<Extension>]
    static member inline foreground(this: WidgetBuilder<'msg, #IFrameworkElement>, value: SolidColorBrush) =
        this.AddScalar(Control.Foreground.WithValue(value))


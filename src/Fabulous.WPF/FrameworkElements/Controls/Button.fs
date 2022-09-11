namespace Fabulous.WPF

open System.Windows.Controls
open Fabulous

type IButton =
    inherit IButtonBase

module Button =
    let WidgetKey = Widgets.register<Button>()
            
    let Background =
        Attributes.defineDependencySolidBrush Button.BackgroundProperty

    let Foreground =
        Attributes.defineDependencySolidBrush Button.ForegroundProperty

    let FontFamily =
        Attributes.defineDependencyWithEquality<string> Button.FontFamilyProperty

    let FontSize =
        Attributes.defineDependencyFloat Button.FontSizeProperty
        
[<AutoOpen>]
module ButtonBuilders =
    type Fabulous.WPF.View with
        static member inline Button<'msg>(text: string, onClicked: 'msg) =
            WidgetBuilder<'msg, IButton>(
                Button.WidgetKey,
                ContentControl.ContentAsString.WithValue(text),
                ButtonBase.Click.WithValue(onClicked)
            )
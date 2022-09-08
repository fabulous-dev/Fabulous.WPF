namespace Fabulous.WPF

open System.Windows.Controls
open Fabulous

type IButton =
    inherit IButtonBase

module Button =
    let WidgetKey = Widgets.register<Button>()

[<AutoOpen>]
module ButtonBuilders =
    type Fabulous.WPF.View with
        static member inline Button<'msg>(text: string, onClicked: 'msg) =
            WidgetBuilder<'msg, IButton>(
                Button.WidgetKey,
                ContentControl.ContentAsString.WithValue(text),
                ButtonBase.Click.WithValue(onClicked)
            )
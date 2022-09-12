namespace Fabulous.WPF

open System.Windows.Controls
open Fabulous

type ICheckBox =
    inherit IToggleButton

module CheckBox =
    let WidgetKey = Widgets.register<CheckBox>()

[<AutoOpen>]
module CheckBoxBuilders =
    type Fabulous.WPF.View with
        static member inline CheckBox<'msg>(text: string, isChecked: bool, onCheckedChanged: bool -> 'msg) =
            WidgetBuilder<'msg, ICheckBox>(
                CheckBox.WidgetKey,
                ContentControl.ContentAsString.WithValue(text),
                ToggleButton.IsCheckedWithEvent.WithValue(ValueEventData.create isChecked (fun args -> onCheckedChanged args |> box))
            )
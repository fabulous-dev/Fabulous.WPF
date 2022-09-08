namespace Fabulous.WPF

open System.Windows.Controls.Primitives

type IToggleButton =
    inherit IButtonBase

module ToggleButton =
    let IsCheckedWithEvent =
        Attributes.defineDependencyWith2RoutedEvents
            "ToggleButton_Checked"
            ToggleButton.IsCheckedProperty
            (fun target -> (target :?> ToggleButton).Checked)
            (fun target -> (target :?> ToggleButton).Unchecked)
            true
            false
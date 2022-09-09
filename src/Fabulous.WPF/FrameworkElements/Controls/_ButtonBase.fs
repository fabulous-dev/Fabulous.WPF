namespace Fabulous.WPF

open System.Windows.Controls.Primitives
open Fabulous

type IButtonBase =
    inherit IContentControl

module ButtonBase =
    let Click = Attributes.defineRoutedEventNoArg "Button_Click" (fun target -> (target :?> ButtonBase).Click)
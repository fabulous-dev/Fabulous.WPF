namespace Fabulous.WPF

open System.Windows.Controls

type IContentControl =
    inherit IControl

module ContentControl =
    let Content = Attributes.defineDependencyWidget ContentControl.ContentProperty

    let ContentAsString = Attributes.defineDependencyWithEquality<string> ContentControl.ContentProperty
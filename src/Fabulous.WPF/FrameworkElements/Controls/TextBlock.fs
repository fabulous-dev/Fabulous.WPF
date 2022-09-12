namespace Fabulous.WPF

open Fabulous
open System.Windows
open System.Windows.Controls
open System.Runtime.CompilerServices

type ITextBlock =
    inherit IFrameworkElement

module TextBlock =
    let WidgetKey = Widgets.register<TextBlock>()

    let Text = Attributes.defineDependencyWithEquality<string> TextBlock.TextProperty

    let FontSize = Attributes.defineDependencyWithEquality<double> TextBlock.FontSizeProperty

    let TextAlignment = Attributes.defineDependencyWithEquality<TextAlignment> TextBlock.TextAlignmentProperty

[<AutoOpen>]
module TextBlockBuilders =
    type Fabulous.WPF.View with
        static member inline TextBlock<'msg>(text: string) =
            WidgetBuilder<'msg, ITextBlock>(TextBlock.WidgetKey, TextBlock.Text.WithValue(text))

[<Extension>]
type TextBlockModifiers =
    [<Extension>]
    static member inline textAlignment(this: WidgetBuilder<'msg, #ITextBlock>, value: TextAlignment) =
        this.AddScalar(TextBlock.TextAlignment.WithValue(value))

    [<Extension>]
    static member inline fontSize(this: WidgetBuilder<'msg, #ITextBlock>, value: double) =
        this.AddScalar(TextBlock.FontSize.WithValue(value))

[<Extension>]
type TextBlockExtraModifiers =
    [<Extension>]
    static member inline centerText(this: WidgetBuilder<'msg, #ITextBlock>) =
        this.textAlignment(TextAlignment.Center)
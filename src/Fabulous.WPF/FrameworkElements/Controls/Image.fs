namespace Fabulous.WPF

open Fabulous
open System.Windows
open System.Windows.Controls
open System.Windows.Media
open System.Runtime.CompilerServices

type IImage =
    inherit IFrameworkElement

module Image =
    let WidgetKey = Widgets.register<Image>()

    let Stretch = Attributes.defineDependencyEnum<Stretch> Image.StretchProperty    
        
    let Source = Attributes.defineDependencyWithEquality<ImageSource> Image.SourceProperty    

[<AutoOpen>]
module ImageBuilders =
    type Fabulous.WPF.View with
        static member inline Image<'msg>(source: ImageSource) =
            WidgetBuilder<'msg, IImage>(
                Image.WidgetKey,
                Image.Source.WithValue(source)
            )

//[<AutoOpen>]
//module TextBlockBuilders =
//    type Fabulous.WPF.View with
//        static member inline TextBlock<'msg>(text: string) =
//            WidgetBuilder<'msg, ITextBlock>(TextBlock.WidgetKey, TextBlock.Text.WithValue(text))

//[<Extension>]
//type TextBlockModifiers =
//    [<Extension>]
//    static member inline textAlignment(this: WidgetBuilder<'msg, #ITextBlock>, value: TextAlignment) =
//        this.AddScalar(TextBlock.TextAlignment.WithValue(value))

//[<Extension>]
//type TextBlockExtraModifiers =
//    [<Extension>]
//    static member inline centerText(this: WidgetBuilder<'msg, #ITextBlock>) =
//        this.textAlignment(TextAlignment.Center)
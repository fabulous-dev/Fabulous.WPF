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

[<Extension>]
type ImageSourceConversion =

    [<Extension>]
    static member inline FromString(imageName: string) : ImageSource =
        let appendedName = "Resources/" + imageName
        ImageSourceConverter().ConvertFromString appendedName :?> ImageSource

namespace Fabulous.WPF

open Fabulous
open System.Windows.Controls
open System.Windows.Media

type IImage =
    inherit IFrameworkElement

module Image =
    let WidgetKey = Widgets.register<Image>()

    let Stretch = Attributes.defineDependencyEnum<Stretch> Image.StretchProperty    
        
    let Source = Attributes.defineDependencyWithEquality<ImageSource> Image.SourceProperty

    /// Keep an instance of ImageSourceConverter here to avoid instantiating this reference on each update
    let private converter = ImageSourceConverter()
    let convertFromString (path: string) = converter.ConvertFromString path :?> ImageSource

[<AutoOpen>]
module ImageBuilders =
    type Fabulous.WPF.View with
        static member inline Image<'msg>(source: ImageSource) =
            WidgetBuilder<'msg, IImage>(
                Image.WidgetKey,
                Image.Source.WithValue(source)
            )

        static member inline Image<'msg>(path: string) =
            View.Image<'msg>(Image.convertFromString path)

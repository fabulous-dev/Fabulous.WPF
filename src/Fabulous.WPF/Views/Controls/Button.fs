namespace Fabulous.WPF

open System.IO
open System.Runtime.CompilerServices
open System.Windows
open System.Windows.Controls
open Fabulous

type IButton =
    inherit IButtonBase

module Button =
    let WidgetKey = Widgets.register<Button>()

    //let BorderColor =
    //    Attributes.defineDependencyAppThemeColor Button.BorderColorProperty

    //let BorderWidth =
    //    Attributes.defineDependencyFloat Button.BorderWidthProperty

    //let CharacterSpacing =
    //    Attributes.defineDependencyFloat Button.CharacterSpacingProperty

    //let ContentLayout =
    //    Attributes.defineDependencyWithEquality<Button.ButtonContentLayout> Button.ContentLayoutProperty

    //let CornerRadius =
    //    Attributes.defineDependencyInt Button.CornerRadiusProperty

    //let FontAttributes =
    //    Attributes.defineDependencyEnum<FontAttributes> Button.FontAttributesProperty
        
    let Background =
        Attributes.defineDependencySolidBrush Button.BackgroundProperty

    let Foreground =
        Attributes.defineDependencySolidBrush Button.ForegroundProperty

    let FontFamily =
        Attributes.defineDependencyWithEquality<string> Button.FontFamilyProperty

    let FontSize =
        Attributes.defineDependencyFloat Button.FontSizeProperty

    //let ImageSource =
    //    Attributes.defineDependencyAppTheme<ImageSource> Button.ImageSourceProperty

    //let Padding =
    //    Attributes.defineDependencyWithEquality<Thickness> Button.PaddingProperty

    //let TextColor =
    //    Attributes.defineDependencyAppThemeColor Button.TextColorProperty

    //let Text =
    //    Attributes.defineDependencyWithEquality<string> Button.TextProperty

    //let TextTransform =
    //    Attributes.defineDependencyEnum<TextTransform> Button.TextTransformProperty

    //let Clicked =
    //    Attributes.defineEventNoArg "Button_Clicked" (fun target -> (target :?> Button).Clicked)

    //let Pressed =
    //    Attributes.defineEventNoArg "Button_Pressed" (fun target -> (target :?> Button).Pressed)

    //let Released =
    //    Attributes.defineEventNoArg "Button_Released" (fun target -> (target :?> Button).Released)

[<AutoOpen>]
module ButtonBuilders =
    type Fabulous.WPF.View with
        static member inline Button<'msg>(text: string, onClicked: 'msg) =
            WidgetBuilder<'msg, IButton>(
                Button.WidgetKey,
                ContentControl.ContentAsString.WithValue(text),
                ButtonBase.Click.WithValue(onClicked)
            )
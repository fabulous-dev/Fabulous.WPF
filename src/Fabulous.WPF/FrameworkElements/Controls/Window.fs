namespace Fabulous.WPF

open System.Windows
open Fabulous
open Fabulous.StackAllocatedCollections.StackList

type IWindow =
    inherit IContentControl

module Window =
    let WidgetKey = Widgets.register<Window>()

    let Title = Attributes.defineDependencyWithEquality<string> Window.TitleProperty

[<AutoOpen>]
module WindowBuilders =
    type Fabulous.WPF.View with
        static member inline Window<'msg, 'marker when 'marker :> IFrameworkElement>(title: string, content: WidgetBuilder<'msg, 'marker>) =
            WidgetBuilder<'msg, IWindow>(
                Window.WidgetKey,
                AttributesBundle(
                    StackList.one(Window.Title.WithValue(title)),
                    ValueSome [| ContentControl.Content.WithValue(content.Compile()) |],
                    ValueNone
                )
            )
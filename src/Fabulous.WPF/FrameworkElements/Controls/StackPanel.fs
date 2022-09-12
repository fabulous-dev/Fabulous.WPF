namespace Fabulous.WPF

open Fabulous
open System.Windows.Controls

type IStackPanel =
    inherit IPanel

module StackPanel =
    let WidgetKey = Widgets.register<StackPanel>()

    let Orientation = Attributes.defineDependencyWithEquality<Orientation> StackPanel.OrientationProperty

[<AutoOpen>]
module StackPanelBuilders =
    type Fabulous.WPF.View with
        static member inline VStack<'msg>() =
            CollectionBuilder<'msg, IStackPanel, IFrameworkElement>(
                StackPanel.WidgetKey,
                Panel.Children,
                StackPanel.Orientation.WithValue(Orientation.Vertical)
            )

        static member inline HStack<'msg>() =
            CollectionBuilder<'msg, IStackPanel, IFrameworkElement>(
                StackPanel.WidgetKey,
                Panel.Children,
                StackPanel.Orientation.WithValue(Orientation.Horizontal)
            )
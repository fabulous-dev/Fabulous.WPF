namespace Fabulous.WPF

open System.Windows.Controls
open Fabulous
open System.Runtime.CompilerServices
open Fabulous.StackAllocatedCollections

type IPanel =
    inherit IFrameworkElement

module Panel =
    let Children =
        Attributes.defineUIElementWidgetCollection
            "Panel_Children"
            (fun target -> (target :?> Panel).Children)

[<Extension>]
type CollectionBuilderExtensions =
    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'marker :> IPanel and 'itemType :> IFrameworkElement>
        (
            _: CollectionBuilder<'msg, 'marker, IFrameworkElement>,
            x: WidgetBuilder<'msg, 'itemType>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }

    [<Extension>]
    static member inline Yield<'msg, 'marker, 'itemType when 'marker :> IPanel and 'itemType :> IFrameworkElement>
        (
            _: CollectionBuilder<'msg, 'marker, IFrameworkElement>,
            x: WidgetBuilder<'msg, Memo.Memoized<'itemType>>
        ) : Content<'msg> =
        { Widgets = MutStackArray1.One(x.Compile()) }
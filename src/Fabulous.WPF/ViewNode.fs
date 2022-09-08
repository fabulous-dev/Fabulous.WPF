namespace Fabulous.WPF

open Fabulous
open System.Windows

module ViewNode =
    let ViewNodeProperty =
        DependencyProperty.Register("ViewNode", typeof<ViewNode>, typeof<FrameworkElement>, null)

    let get (target: obj) =
        // This one is a hack because Fabulous 2.0.7 try to retrieve a value of ViewNode from another thread
        Application.Current.Dispatcher.Invoke(
            fun () ->
                (target :?> FrameworkElement)
                    .GetValue(ViewNodeProperty)
                :?> IViewNode
        )

    let set (node: IViewNode) (target: obj) =
        (target :?> FrameworkElement)
            .SetValue(ViewNodeProperty, node)
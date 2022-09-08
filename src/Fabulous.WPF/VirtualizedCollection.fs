namespace Fabulous.WPF

open System
open System.Collections.Generic
open Fabulous
open System.Windows
open System.Windows.Controls

module BindableHelpers =
    /// On DataContextChanged triggered, call the Reconciler to update the cell
    let createOnDataContextChanged canReuseView (getViewNode: obj -> IViewNode) templateFn (target: FrameworkElement) =
        let mutable prevWidgetOpt: Widget voption = ValueNone

        let onDataContextChanged () =
            match target.DataContext with
            | null -> ()
            | value ->
                let currWidget = templateFn value

                let node = getViewNode target
                Reconciler.update canReuseView prevWidgetOpt currWidget node
                prevWidgetOpt <- ValueSome currWidget

        onDataContextChanged

/// Create a DataTemplate for a specific root type (TextCell, ViewCell, etc.)
/// that listen for DataContext change to apply the Widget content to the cell
type WidgetDataTemplate(parent: IViewNode, ``type``: Type, templateFn: obj -> Widget) =
    inherit DataTemplate(fun () ->
        let frameworkElement =
            Activator.CreateInstance ``type`` :?> FrameworkElement

        let viewNode =
            ViewNode(Some parent, parent.TreeContext, WeakReference(frameworkElement))

        frameworkElement.SetValue(ViewNode.ViewNodeProperty, viewNode)

        let onDataContextChanged =
            BindableHelpers.createOnDataContextChanged
                parent.TreeContext.CanReuseView
                parent.TreeContext.GetViewNode
                templateFn
                frameworkElement

        frameworkElement.DataContextChanged.Add(fun _ -> onDataContextChanged())

        frameworkElement :> obj)

/// Redirect to the right type of DataTemplate based on the target type of the current widget cell
type WidgetDataTemplateSelector internal (node: IViewNode, templateFn: obj -> Widget) =
    inherit DataTemplateSelector()

    /// Reuse data template for already known widget target type
    let cache = Dictionary<Type, DataTemplate>()

    override _.SelectTemplate(item, _) =
        let widget = templateFn item
        let widgetDefinition = WidgetDefinitionStore.get widget.Key
        let targetType = widgetDefinition.TargetType

        match cache.TryGetValue(targetType) with
        | true, dataTemplate -> dataTemplate
        | false, _ ->
            let dataTemplate =
                WidgetDataTemplate(node, targetType, templateFn) :> DataTemplate

            cache.Add(targetType, dataTemplate)
            dataTemplate

type WidgetItems<'T> =
    { OriginalItems: IEnumerable<'T>
      Template: 'T -> Widget }

type GroupedWidgetItems<'groupData, 'itemData> =
    { OriginalItems: IEnumerable<'groupData>
      HeaderTemplate: 'groupData -> Widget
      FooterTemplate: ('groupData -> Widget) option
      ItemTemplate: 'itemData -> Widget }
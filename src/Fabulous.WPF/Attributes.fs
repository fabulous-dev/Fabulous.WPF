namespace Fabulous.WPF

open Fabulous
open Fabulous.ScalarAttributeDefinitions
open System
open System.Runtime.CompilerServices
open System.Windows
open System.Windows.Media

[<Struct>]
type ValueEventData<'data, 'eventArgs> =
    { Value: 'data
      Event: 'eventArgs -> obj }

module ValueEventData =
    let create (value: 'data) (event: 'eventArgs -> obj) = { Value = value; Event = event }

/// WPF specific attributes that can be encoded as 8 bytes
module SmallScalars =
    module FabColor =
        let inline encode (v: FabColor) : uint64 = SmallScalars.UInt.encode v.RGBA

        let inline decode (encoded: uint64) : FabColor =
            { RGBA = SmallScalars.UInt.decode encoded }

        let inline encodeSolidBrush (v: SolidColorBrush) : uint64 =
            let fabColor = ColorConversion.ToFabColor v
            SmallScalars.UInt.encode fabColor.RGBA             
        
        let inline decodeSolidBrush (encoded: uint64) : SolidColorBrush =
            let fabColor = { RGBA = SmallScalars.UInt.decode encoded }
            ColorConversion.ToSolidBrush fabColor

[<Extension>]
type SmallScalarExtensions() =
    [<Extension>]
    static member inline WithValue(this: SmallScalarAttributeDefinition<bool>, value) =
        this.WithValue(value, SmallScalars.Bool.encode)

    [<Extension>]
    static member inline WithValue(this: SmallScalarAttributeDefinition<float>, value) =
        this.WithValue(value, SmallScalars.Float.encode)

    [<Extension>]
    static member inline WithValue(this: SmallScalarAttributeDefinition<int>, value) =
        this.WithValue(value, SmallScalars.Int.encode)

    [<Extension>]
    static member inline WithValue(this: SmallScalarAttributeDefinition<SolidColorBrush>, value) =
        this.WithValue(value, SmallScalars.FabColor.encodeSolidBrush)

    [<Extension>]
    static member inline WithValue< ^T when ^T: enum<int> and ^T: (static member op_Explicit: ^T -> uint64)>
        (
            this: SmallScalarAttributeDefinition< ^T >,
            value
        ) =
        this.WithValue(value, SmallScalars.IntEnum.encode)


module Attributes =
    /// Define an attribute for a DependencyProperty
    let inline defineDependency<'modelType, 'valueType>
        (dependencyProperty: DependencyProperty)
        ([<InlineIfLambda>] convertValue: 'modelType -> 'valueType)
        ([<InlineIfLambda>] compare: 'modelType -> 'modelType -> ScalarAttributeComparison)
        =
        Attributes.defineScalar<'modelType, 'valueType>
            dependencyProperty.Name
            convertValue
            compare
            (fun _ newValueOpt node ->
                let target = node.Target :?> FrameworkElement

                match newValueOpt with
                | ValueNone -> target.ClearValue(dependencyProperty)
                | ValueSome v -> target.SetValue(dependencyProperty, v))

    /// Define an attribute for a DependencyProperty supporting equality comparison
    let inline defineDependencyWithEquality<'T when 'T: equality> (dependencyProperty: DependencyProperty) =
        Attributes.defineSimpleScalarWithEquality<'T>
            dependencyProperty.Name
            (fun _ newValueOpt node ->
                let target = node.Target :?> FrameworkElement

                match newValueOpt with
                | ValueNone -> target.ClearValue(dependencyProperty)
                | ValueSome v -> target.SetValue(dependencyProperty, v))

    /// Define an attribute that can fit into 8 bytes encoded as uint64 (such as float or bool) for a DependencyProperty
    let inline defineSmallDependency<'T> (dependencyProperty: DependencyProperty) ([<InlineIfLambda>] decode: uint64 -> 'T) =
        Attributes.defineSmallScalar<'T>
            dependencyProperty.Name
            decode
            (fun _ newValueOpt node ->
                let target = node.Target :?> FrameworkElement

                match newValueOpt with
                | ValueNone -> target.ClearValue(dependencyProperty)
                | ValueSome v -> target.SetValue(dependencyProperty, v))

    /// Define a float attribute for a DependencyProperty and encode it as a small scalar (8 bytes)
    let inline defineDependencyFloat (dependencyProperty: DependencyProperty) =
        defineSmallDependency dependencyProperty SmallScalars.Float.decode

    /// Define a boolean attribute for a DependencyProperty and encode it as a small scalar (8 bytes)
    let inline defineDependencyBool (dependencyProperty: DependencyProperty) =
        defineSmallDependency dependencyProperty SmallScalars.Bool.decode

    /// Define an int attribute for a DependencyProperty and encode it as a small scalar (8 bytes)
    let inline defineDependencyInt (dependencyProperty: DependencyProperty) =
        defineSmallDependency dependencyProperty SmallScalars.Int.decode

    /// Define a Color attribute for a DependencyProperty and encode it as a small scalar (8 bytes).
    /// Note that the input type is FabColor because it is just 4 bytes
    /// But it converts back to Windows.Media.Color when a value is applied
    /// Note if you want to use Windows.Media.Color directly you can do that with "defineDependency".
    /// However, WPF.Color will be boxed and thus slower.
    let inline defineDependencyColor (dependencyProperty: DependencyProperty) : SmallScalarAttributeDefinition<FabColor> =
        Attributes.defineSmallScalar<FabColor>
            dependencyProperty.Name
            SmallScalars.FabColor.decode
            (fun _ newValueOpt node ->
                let target = node.Target :?> DependencyObject

                match newValueOpt with
                | ValueNone -> target.ClearValue(dependencyProperty)
                | ValueSome v -> target.SetValue(dependencyProperty, v.ToWPFColor()))

    let inline defineDependencySolidBrush (dependencyProperty: DependencyProperty) : SmallScalarAttributeDefinition<SolidColorBrush> =
        Attributes.defineSmallScalar<SolidColorBrush>
            dependencyProperty.Name
            SmallScalars.FabColor.decodeSolidBrush
            (fun _ newValueOpt node ->
                let target = node.Target :?> DependencyObject

                match newValueOpt with
                | ValueNone -> target.ClearValue(dependencyProperty)
                | ValueSome v -> target.SetValue(dependencyProperty, v))

    /// Define an enum attribute for a DependencyProperty and encode it as a small scalar (8 bytes)
    let inline defineDependencyEnum< ^T when ^T: enum<int>>
        (dependencyProperty: DependencyProperty)
        : SmallScalarAttributeDefinition< ^T > =
        Attributes.defineEnum< ^T>
            dependencyProperty.Name
            (fun _ newValueOpt node ->
                let target = node.Target :?> FrameworkElement

                match newValueOpt with
                | ValueNone -> target.ClearValue(dependencyProperty)
                | ValueSome v -> target.SetValue(dependencyProperty, v))

    /// Define an attribute storing a Widget for a bindable property
    let inline defineDependencyWidget (dependencyProperty: DependencyProperty) =
        Attributes.definePropertyWidget
            dependencyProperty.Name
            (fun target ->
                (target :?> FrameworkElement)
                    .GetValue(dependencyProperty))
            (fun target value ->
                let bindableObject = target :?> FrameworkElement

                if value = null then
                    bindableObject.ClearValue(dependencyProperty)
                else
                    bindableObject.SetValue(dependencyProperty, value))

    /// Update both a property and its related event.
    /// This definition makes sure that the event is only raised when the property is changed by the user,
    /// and not when the property is set by the code
    let defineDependencyWithEvent<'data, 'args>
        name
        (dependencyProperty: DependencyProperty)
        (getEvent: obj -> IEvent<EventHandler<'args>, 'args>)
        : SimpleScalarAttributeDefinition<ValueEventData<'data, 'args>> =

        let key =
            SimpleScalarAttributeDefinition.CreateAttributeData(
                ScalarAttributeComparers.noCompare,
                (fun oldValueOpt (newValueOpt: ValueEventData<'data, 'args> voption) node ->
                    let target = node.Target :?> FrameworkElement
                    let event = getEvent target

                    match newValueOpt with
                    | ValueNone ->
                        // The attribute is no longer applied, so we clean up the event
                        match node.TryGetHandler(name) with
                        | ValueNone -> ()
                        | ValueSome handler -> event.RemoveHandler(handler)

                        // Only clear the property if a value was set before
                        match oldValueOpt with
                        | ValueNone -> ()
                        | ValueSome _ -> target.ClearValue(dependencyProperty)

                    | ValueSome curr ->
                        // Clean up the old event handler if any
                        match node.TryGetHandler(name) with
                        | ValueNone -> ()
                        | ValueSome handler -> event.RemoveHandler(handler)

                        // Set the new value
                        target.SetValue(dependencyProperty, curr.Value)

                        // Set the new event handler
                        let handler =
                            EventHandler<'args>
                                (fun _ args ->
                                    let r = curr.Event args
                                    Dispatcher.dispatch node r)

                        node.SetHandler(name, ValueSome handler)
                        event.AddHandler(handler))
            )
            |> AttributeDefinitionStore.registerScalar

        { Key = key; Name = name }

    /// Update both a property and its related event.
    /// This definition makes sure that the event is only raised when the property is changed by the user,
    /// and not when the property is set by the code
    let defineDependencyWithRoutedPropertyChangedEvent<'data>
        name
        (dependencyProperty: DependencyProperty)
        (getEvent: obj -> IEvent<RoutedPropertyChangedEventHandler<'data>, RoutedPropertyChangedEventArgs<'data>>)
        : SimpleScalarAttributeDefinition<ValueEventData<'data, RoutedPropertyChangedEventArgs<'data>>> =

        let key =
            SimpleScalarAttributeDefinition.CreateAttributeData(
                ScalarAttributeComparers.noCompare,
                (fun oldValueOpt (newValueOpt: ValueEventData<'data, RoutedPropertyChangedEventArgs<'data>> voption) node ->
                    let target = node.Target :?> FrameworkElement
                    let event = getEvent target

                    match newValueOpt with
                    | ValueNone ->
                        // The attribute is no longer applied, so we clean up the event
                        match node.TryGetHandler(name) with
                        | ValueNone -> ()
                        | ValueSome handler -> event.RemoveHandler(handler)

                        // Only clear the property if a value was set before
                        match oldValueOpt with
                        | ValueNone -> ()
                        | ValueSome _ -> target.ClearValue(dependencyProperty)

                    | ValueSome curr ->
                        // Clean up the old event handler if any
                        match node.TryGetHandler(name) with
                        | ValueNone -> ()
                        | ValueSome handler -> event.RemoveHandler(handler)

                        // Set the new value
                        target.SetValue(dependencyProperty, curr.Value)

                        // Set the new event handler
                        let handler =
                            RoutedPropertyChangedEventHandler<'data>
                                (fun _ args ->
                                    let r = curr.Event args
                                    Dispatcher.dispatch node r)

                        node.SetHandler(name, ValueSome handler)
                        event.AddHandler(handler))
            )
            |> AttributeDefinitionStore.registerScalar

        { Key = key; Name = name }

    /// Update both a property and its related event.
    /// This definition makes sure that the event is only raised when the property is changed by the user,
    /// and not when the property is set by the code
    let defineDependencyWith2RoutedEvents<'data>
        name
        (dependencyProperty: DependencyProperty)
        (getEventOn: obj -> IEvent<RoutedEventHandler, RoutedEventArgs>)
        (getEventOff: obj -> IEvent<RoutedEventHandler, RoutedEventArgs>)
        (valueOn: 'data)
        (valueOff: 'data)
        : SimpleScalarAttributeDefinition<ValueEventData<'data, 'data>> =

        let key =
            SimpleScalarAttributeDefinition.CreateAttributeData(
                ScalarAttributeComparers.noCompare,
                (fun oldValueOpt (newValueOpt: ValueEventData<'data, 'data> voption) node ->
                    let target = node.Target :?> FrameworkElement
                    let eventOn = getEventOn target
                    let eventOff = getEventOff target

                    let eventOnName = $"{name}_On"
                    let eventOffName = $"{name}_Off"

                    match newValueOpt with
                    | ValueNone ->
                        // The attribute is no longer applied, so we clean up the event
                        match node.TryGetHandler(eventOnName) with
                        | ValueNone -> ()
                        | ValueSome handler -> eventOn.RemoveHandler(handler)

                        match node.TryGetHandler(eventOffName) with
                        | ValueNone -> ()
                        | ValueSome handler -> eventOff.RemoveHandler(handler)

                        // Only clear the property if a value was set before
                        match oldValueOpt with
                        | ValueNone -> ()
                        | ValueSome _ -> target.ClearValue(dependencyProperty)

                    | ValueSome curr ->
                        // Clean up the old event handler if any
                        match node.TryGetHandler(eventOnName) with
                        | ValueNone -> ()
                        | ValueSome handler -> eventOn.RemoveHandler(handler)

                        match node.TryGetHandler(eventOffName) with
                        | ValueNone -> ()
                        | ValueSome handler -> eventOff.RemoveHandler(handler)

                        // Set the new value
                        target.SetValue(dependencyProperty, curr.Value)

                        // Set the new event handler
                        let handlerOn =
                            RoutedEventHandler
                                (fun _ _ ->
                                    let r = curr.Event valueOn
                                    Dispatcher.dispatch node r)

                        let handlerOff =
                            RoutedEventHandler
                                (fun _ _ ->
                                    let r = curr.Event valueOff
                                    Dispatcher.dispatch node r)

                        node.SetHandler(eventOnName, ValueSome handlerOn)
                        eventOn.AddHandler(handlerOn)

                        node.SetHandler(eventOffName, ValueSome handlerOff)
                        eventOff.AddHandler(handlerOff))
            )
            |> AttributeDefinitionStore.registerScalar

        { Key = key; Name = name }


    let inline defineUIElementWidgetCollection
        name
        ([<InlineIfLambda>] getCollection: obj -> System.Windows.Controls.UIElementCollection)
        =
        let applyDiff _ (diffs: WidgetCollectionItemChanges) (node: IViewNode) =
            let targetColl = getCollection node.Target

            for diff in diffs do
                match diff with
                | WidgetCollectionItemChange.Remove (index, widget) ->
                    let itemNode =
                        node.TreeContext.GetViewNode(box targetColl.[index])

                    // Trigger the unmounted event
                    Dispatcher.dispatchEventForAllChildren itemNode widget Lifecycle.Unmounted
                    itemNode.Disconnect()

                    // Remove the child from the UI tree
                    targetColl.RemoveAt(index)

                | _ -> ()

            for diff in diffs do
                match diff with
                | WidgetCollectionItemChange.Insert (index, widget) ->
                    let struct (itemNode, view) = Helpers.createViewForWidget node widget

                    // Insert the new child into the UI tree
                    targetColl.Insert(index, unbox view)

                    // Trigger the mounted event
                    Dispatcher.dispatchEventForAllChildren itemNode widget Lifecycle.Mounted

                | WidgetCollectionItemChange.Update (index, widgetDiff) ->
                    let childNode =
                        node.TreeContext.GetViewNode(box targetColl.[index])

                    childNode.ApplyDiff(&widgetDiff)

                | WidgetCollectionItemChange.Replace (index, oldWidget, newWidget) ->
                    let prevItemNode =
                        node.TreeContext.GetViewNode(box targetColl.[index])

                    let struct (nextItemNode, view) =
                        Helpers.createViewForWidget node newWidget

                    // Trigger the unmounted event for the old child
                    Dispatcher.dispatchEventForAllChildren prevItemNode oldWidget Lifecycle.Unmounted
                    prevItemNode.Disconnect()

                    // Replace the existing child in the UI tree at the index with the new one
                    targetColl.[index] <- unbox view

                    // Trigger the mounted event for the new child
                    Dispatcher.dispatchEventForAllChildren nextItemNode newWidget Lifecycle.Mounted

                | _ -> ()

        let updateNode _ (newValueOpt: ArraySlice<Widget> voption) (node: IViewNode) =
            let targetColl = getCollection node.Target
            targetColl.Clear()

            match newValueOpt with
            | ValueNone -> ()
            | ValueSome widgets ->
                for widget in ArraySlice.toSpan widgets do
                    let struct (_, view) = Helpers.createViewForWidget node widget

                    targetColl.Add(unbox view) |> ignore

        Attributes.defineWidgetCollection name applyDiff updateNode

    /// Define an attribute for RoutedEventHandler
    let inline defineRoutedEventNoArg
        name
        ([<InlineIfLambda>] getEvent: obj -> IEvent<RoutedEventHandler, RoutedEventArgs>)
        : SimpleScalarAttributeDefinition<obj> =
        let key =
            SimpleScalarAttributeDefinition.CreateAttributeData(
                ScalarAttributeComparers.noCompare,
                (fun _ newValueOpt node ->
                    let event = getEvent node.Target

                    match node.TryGetHandler(name) with
                    | ValueNone -> ()
                    | ValueSome handler -> event.RemoveHandler handler

                    match newValueOpt with
                    | ValueNone -> node.SetHandler(name, ValueNone)

                    | ValueSome msg ->
                        let handler =
                            RoutedEventHandler(fun _ _ -> Dispatcher.dispatch node msg)

                        event.AddHandler handler
                        node.SetHandler(name, ValueSome handler))
            )

            |> AttributeDefinitionStore.registerScalar

        { Key = key; Name = name }
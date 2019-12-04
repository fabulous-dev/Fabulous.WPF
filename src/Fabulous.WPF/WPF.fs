// Copyright 2018-2019 Fabulous contributors. See LICENSE.md for license.
namespace Fabulous.WPF

#nowarn "59" // cast always holds
#nowarn "66" // cast always holds
#nowarn "67" // cast always holds
#nowarn "46" // `checked` is a correct name 

open Fabulous

module ViewAttributes =
    let HorizontalAlignmentAttribKey : AttributeKey<_> = AttributeKey<_>("HorizontalAlignment")
    let MarginAttribKey : AttributeKey<_> = AttributeKey<_>("Margin")
    let VerticalAlignmentAttribKey : AttributeKey<_> = AttributeKey<_>("VerticalAlignment")
    let WidthAttribKey : AttributeKey<_> = AttributeKey<_>("Width")
    let ContentControlContentAttribKey : AttributeKey<_> = AttributeKey<_>("ContentControlContent")
    let TitleAttribKey : AttributeKey<_> = AttributeKey<_>("Title")
    let CommandAttribKey : AttributeKey<_> = AttributeKey<_>("Command")
    let CommandCanExecuteAttribKey : AttributeKey<_> = AttributeKey<_>("CommandCanExecute")
    let ButtonContentAttribKey : AttributeKey<_> = AttributeKey<_>("ButtonContent")
    let CheckedAttribKey : AttributeKey<_> = AttributeKey<_>("Checked")
    let UncheckedAttribKey : AttributeKey<_> = AttributeKey<_>("Unchecked")
    let IsCheckedAttribKey : AttributeKey<_> = AttributeKey<_>("IsChecked")
    let TextAttribKey : AttributeKey<_> = AttributeKey<_>("Text")
    let TextAlignmentAttribKey : AttributeKey<_> = AttributeKey<_>("TextAlignment")
    let ValueChangedAttribKey : AttributeKey<_> = AttributeKey<_>("ValueChanged")
    let MinimumAttribKey : AttributeKey<_> = AttributeKey<_>("Minimum")
    let MaximumAttribKey : AttributeKey<_> = AttributeKey<_>("Maximum")
    let ValueAttribKey : AttributeKey<_> = AttributeKey<_>("Value")
    let ChildrenAttribKey : AttributeKey<_> = AttributeKey<_>("Children")
    let OrientationAttribKey : AttributeKey<_> = AttributeKey<_>("Orientation")

type ViewBuilders() =
    /// Builds the attributes for a FrameworkElement in the view
    static member inline BuildFrameworkElement(attribCount: int,
                                               ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                               ?margin: System.Windows.Thickness,
                                               ?verticalAlignment: System.Windows.VerticalAlignment,
                                               ?width: float) = 

        let attribCount = match horizontalAlignment with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match margin with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match verticalAlignment with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match width with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = new AttributesBuilder(attribCount)
        match horizontalAlignment with None -> () | Some v -> attribBuilder.Add(ViewAttributes.HorizontalAlignmentAttribKey, (v)) 
        match margin with None -> () | Some v -> attribBuilder.Add(ViewAttributes.MarginAttribKey, (v)) 
        match verticalAlignment with None -> () | Some v -> attribBuilder.Add(ViewAttributes.VerticalAlignmentAttribKey, (v)) 
        match width with None -> () | Some v -> attribBuilder.Add(ViewAttributes.WidthAttribKey, (v)) 
        attribBuilder

    static member UpdateFrameworkElement (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.FrameworkElement) = 
        let mutable prevHorizontalAlignmentOpt = ValueNone
        let mutable currHorizontalAlignmentOpt = ValueNone
        let mutable prevMarginOpt = ValueNone
        let mutable currMarginOpt = ValueNone
        let mutable prevVerticalAlignmentOpt = ValueNone
        let mutable currVerticalAlignmentOpt = ValueNone
        let mutable prevWidthOpt = ValueNone
        let mutable currWidthOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.HorizontalAlignmentAttribKey.KeyValue then 
                currHorizontalAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.HorizontalAlignment)
            if kvp.Key = ViewAttributes.MarginAttribKey.KeyValue then 
                currMarginOpt <- ValueSome (kvp.Value :?> System.Windows.Thickness)
            if kvp.Key = ViewAttributes.VerticalAlignmentAttribKey.KeyValue then 
                currVerticalAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.VerticalAlignment)
            if kvp.Key = ViewAttributes.WidthAttribKey.KeyValue then 
                currWidthOpt <- ValueSome (kvp.Value :?> float)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.HorizontalAlignmentAttribKey.KeyValue then 
                    prevHorizontalAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.HorizontalAlignment)
                if kvp.Key = ViewAttributes.MarginAttribKey.KeyValue then 
                    prevMarginOpt <- ValueSome (kvp.Value :?> System.Windows.Thickness)
                if kvp.Key = ViewAttributes.VerticalAlignmentAttribKey.KeyValue then 
                    prevVerticalAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.VerticalAlignment)
                if kvp.Key = ViewAttributes.WidthAttribKey.KeyValue then 
                    prevWidthOpt <- ValueSome (kvp.Value :?> float)
        // Update properties
        match prevHorizontalAlignmentOpt, currHorizontalAlignmentOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.HorizontalAlignment <-  currValue
        | ValueSome _, ValueNone -> target.HorizontalAlignment <- System.Windows.HorizontalAlignment.Stretch
        | ValueNone, ValueNone -> ()
        match prevMarginOpt, currMarginOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Margin <-  currValue
        | ValueSome _, ValueNone -> target.Margin <- System.Windows.Thickness(0.)
        | ValueNone, ValueNone -> ()
        match prevVerticalAlignmentOpt, currVerticalAlignmentOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.VerticalAlignment <-  currValue
        | ValueSome _, ValueNone -> target.VerticalAlignment <- System.Windows.VerticalAlignment.Stretch
        | ValueNone, ValueNone -> ()
        match prevWidthOpt, currWidthOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Width <-  currValue
        | ValueSome _, ValueNone -> target.Width <- System.Double.NaN
        | ValueNone, ValueNone -> ()

    /// Builds the attributes for a ContentControl in the view
    static member inline BuildContentControl(attribCount: int,
                                             ?content: ViewElement,
                                             ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                             ?margin: System.Windows.Thickness,
                                             ?verticalAlignment: System.Windows.VerticalAlignment,
                                             ?width: float) = 

        let attribCount = match content with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = ViewBuilders.BuildFrameworkElement(attribCount, ?horizontalAlignment=horizontalAlignment, ?margin=margin, ?verticalAlignment=verticalAlignment, ?width=width)
        match content with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ContentControlContentAttribKey, (v)) 
        attribBuilder

    static member CreateContentControl () : System.Windows.Controls.ContentControl =
        new System.Windows.Controls.ContentControl()

    static member UpdateContentControl (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.ContentControl) = 
        let mutable prevContentControlContentOpt = ValueNone
        let mutable currContentControlContentOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.ContentControlContentAttribKey.KeyValue then 
                currContentControlContentOpt <- ValueSome (kvp.Value :?> ViewElement)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.ContentControlContentAttribKey.KeyValue then 
                    prevContentControlContentOpt <- ValueSome (kvp.Value :?> ViewElement)
        // Update inherited members
        ViewBuilders.UpdateFrameworkElement (prevOpt, curr, target)
        // Update properties
        match prevContentControlContentOpt, currContentControlContentOpt with
        // For structured objects, dependsOn on reference equality
        | ValueSome prevValue, ValueSome newValue when identical prevValue newValue -> ()
        | ValueSome prevValue, ValueSome newValue when canReuseView prevValue newValue ->
            newValue.UpdateIncremental(prevValue, target.Content)
        | _, ValueSome newValue ->
            target.Content <- (newValue.Create() :?> obj)
        | ValueSome _, ValueNone ->
            target.Content <- null
        | ValueNone, ValueNone -> ()

    static member inline ConstructContentControl(?content: ViewElement,
                                                 ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                                 ?margin: System.Windows.Thickness,
                                                 ?verticalAlignment: System.Windows.VerticalAlignment,
                                                 ?width: float) = 

        let attribBuilder = ViewBuilders.BuildContentControl(0,
                               ?content=content,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

        ViewElement.Create<System.Windows.Controls.ContentControl>(ViewBuilders.CreateContentControl, (fun prevOpt curr target -> ViewBuilders.UpdateContentControl(prevOpt, curr, target)), attribBuilder)

    /// Builds the attributes for a Window in the view
    static member inline BuildWindow(attribCount: int,
                                     ?title: string,
                                     ?content: ViewElement,
                                     ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                     ?margin: System.Windows.Thickness,
                                     ?verticalAlignment: System.Windows.VerticalAlignment,
                                     ?width: float) = 

        let attribCount = match title with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = ViewBuilders.BuildContentControl(attribCount, ?content=content, ?horizontalAlignment=horizontalAlignment, ?margin=margin, ?verticalAlignment=verticalAlignment, ?width=width)
        match title with None -> () | Some v -> attribBuilder.Add(ViewAttributes.TitleAttribKey, (v)) 
        attribBuilder

    static member CreateWindow () : System.Windows.Window =
        new System.Windows.Window()

    static member UpdateWindow (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Window) = 
        let mutable prevTitleOpt = ValueNone
        let mutable currTitleOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.TitleAttribKey.KeyValue then 
                currTitleOpt <- ValueSome (kvp.Value :?> string)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.TitleAttribKey.KeyValue then 
                    prevTitleOpt <- ValueSome (kvp.Value :?> string)
        // Update inherited members
        ViewBuilders.UpdateContentControl (prevOpt, curr, target)
        // Update properties
        match prevTitleOpt, currTitleOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Title <-  currValue
        | ValueSome _, ValueNone -> target.Title <- System.String.Empty
        | ValueNone, ValueNone -> ()

    static member inline ConstructWindow(?title: string,
                                         ?content: ViewElement,
                                         ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                         ?margin: System.Windows.Thickness,
                                         ?verticalAlignment: System.Windows.VerticalAlignment,
                                         ?width: float) = 

        let attribBuilder = ViewBuilders.BuildWindow(0,
                               ?title=title,
                               ?content=content,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

        ViewElement.Create<System.Windows.Window>(ViewBuilders.CreateWindow, (fun prevOpt curr target -> ViewBuilders.UpdateWindow(prevOpt, curr, target)), attribBuilder)

    /// Builds the attributes for a ButtonBase in the view
    static member inline BuildButtonBase(attribCount: int,
                                         ?command: unit -> unit,
                                         ?commandCanExecute: bool,
                                         ?content: ViewElement,
                                         ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                         ?margin: System.Windows.Thickness,
                                         ?verticalAlignment: System.Windows.VerticalAlignment,
                                         ?width: float) = 

        let attribCount = match command with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match commandCanExecute with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = ViewBuilders.BuildContentControl(attribCount, ?content=content, ?horizontalAlignment=horizontalAlignment, ?margin=margin, ?verticalAlignment=verticalAlignment, ?width=width)
        match command with None -> () | Some v -> attribBuilder.Add(ViewAttributes.CommandAttribKey, (v)) 
        match commandCanExecute with None -> () | Some v -> attribBuilder.Add(ViewAttributes.CommandCanExecuteAttribKey, (v)) 
        attribBuilder

    static member UpdateButtonBase (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.Primitives.ButtonBase) = 
        let mutable prevCommandOpt = ValueNone
        let mutable currCommandOpt = ValueNone
        let mutable prevCommandCanExecuteOpt = ValueNone
        let mutable currCommandCanExecuteOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.CommandAttribKey.KeyValue then 
                currCommandOpt <- ValueSome (kvp.Value :?> unit -> unit)
            if kvp.Key = ViewAttributes.CommandCanExecuteAttribKey.KeyValue then 
                currCommandCanExecuteOpt <- ValueSome (kvp.Value :?> bool)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.CommandAttribKey.KeyValue then 
                    prevCommandOpt <- ValueSome (kvp.Value :?> unit -> unit)
                if kvp.Key = ViewAttributes.CommandCanExecuteAttribKey.KeyValue then 
                    prevCommandCanExecuteOpt <- ValueSome (kvp.Value :?> bool)
        // Update inherited members
        ViewBuilders.UpdateContentControl (prevOpt, curr, target)
        // Update properties
        (fun _ _ _ -> ()) prevCommandOpt currCommandOpt target
        ViewUpdaters.updateCommand prevCommandOpt currCommandOpt (fun _target -> ()) (fun (target: System.Windows.Controls.Primitives.ButtonBase) cmd -> target.Command <- cmd) prevCommandCanExecuteOpt currCommandCanExecuteOpt target

    /// Builds the attributes for a Button in the view
    static member inline BuildButton(attribCount: int,
                                     ?content: string,
                                     ?command: unit -> unit,
                                     ?commandCanExecute: bool,
                                     ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                     ?margin: System.Windows.Thickness,
                                     ?verticalAlignment: System.Windows.VerticalAlignment,
                                     ?width: float) = 

        let attribCount = match content with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = ViewBuilders.BuildButtonBase(attribCount, ?command=command, ?commandCanExecute=commandCanExecute, ?horizontalAlignment=horizontalAlignment, ?margin=margin, ?verticalAlignment=verticalAlignment, 
                                                         ?width=width)
        match content with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ButtonContentAttribKey, (v)) 
        attribBuilder

    static member CreateButton () : System.Windows.Controls.Button =
        new System.Windows.Controls.Button()

    static member UpdateButton (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.Button) = 
        let mutable prevButtonContentOpt = ValueNone
        let mutable currButtonContentOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.ButtonContentAttribKey.KeyValue then 
                currButtonContentOpt <- ValueSome (kvp.Value :?> string)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.ButtonContentAttribKey.KeyValue then 
                    prevButtonContentOpt <- ValueSome (kvp.Value :?> string)
        // Update inherited members
        ViewBuilders.UpdateButtonBase (prevOpt, curr, target)
        // Update properties
        match prevButtonContentOpt, currButtonContentOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Content <-  currValue
        | ValueSome _, ValueNone -> target.Content <- null
        | ValueNone, ValueNone -> ()

    static member inline ConstructButton(?content: string,
                                         ?command: unit -> unit,
                                         ?commandCanExecute: bool,
                                         ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                         ?margin: System.Windows.Thickness,
                                         ?verticalAlignment: System.Windows.VerticalAlignment,
                                         ?width: float) = 

        let attribBuilder = ViewBuilders.BuildButton(0,
                               ?content=content,
                               ?command=command,
                               ?commandCanExecute=commandCanExecute,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

        ViewElement.Create<System.Windows.Controls.Button>(ViewBuilders.CreateButton, (fun prevOpt curr target -> ViewBuilders.UpdateButton(prevOpt, curr, target)), attribBuilder)

    /// Builds the attributes for a ToggleButton in the view
    static member inline BuildToggleButton(attribCount: int,
                                           ?isChecked: bool option,
                                           ?command: unit -> unit,
                                           ?commandCanExecute: bool,
                                           ?content: ViewElement,
                                           ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                           ?margin: System.Windows.Thickness,
                                           ?verticalAlignment: System.Windows.VerticalAlignment,
                                           ?width: float,
                                           ?checked: unit -> unit,
                                           ?unchecked: unit -> unit) = 

        let attribCount = match isChecked with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match checked with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match unchecked with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = ViewBuilders.BuildButtonBase(attribCount, ?command=command, ?commandCanExecute=commandCanExecute, ?content=content, ?horizontalAlignment=horizontalAlignment, ?margin=margin, 
                                                         ?verticalAlignment=verticalAlignment, ?width=width)
        match isChecked with None -> () | Some v -> attribBuilder.Add(ViewAttributes.IsCheckedAttribKey, (v)) 
        match checked with None -> () | Some v -> attribBuilder.Add(ViewAttributes.CheckedAttribKey, (fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))(v)) 
        match unchecked with None -> () | Some v -> attribBuilder.Add(ViewAttributes.UncheckedAttribKey, (fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))(v)) 
        attribBuilder

    static member UpdateToggleButton (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.Primitives.ToggleButton) = 
        let mutable prevCheckedOpt = ValueNone
        let mutable currCheckedOpt = ValueNone
        let mutable prevUncheckedOpt = ValueNone
        let mutable currUncheckedOpt = ValueNone
        let mutable prevIsCheckedOpt = ValueNone
        let mutable currIsCheckedOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.CheckedAttribKey.KeyValue then 
                currCheckedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedEventHandler)
            if kvp.Key = ViewAttributes.UncheckedAttribKey.KeyValue then 
                currUncheckedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedEventHandler)
            if kvp.Key = ViewAttributes.IsCheckedAttribKey.KeyValue then 
                currIsCheckedOpt <- ValueSome (kvp.Value :?> bool option)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.CheckedAttribKey.KeyValue then 
                    prevCheckedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedEventHandler)
                if kvp.Key = ViewAttributes.UncheckedAttribKey.KeyValue then 
                    prevUncheckedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedEventHandler)
                if kvp.Key = ViewAttributes.IsCheckedAttribKey.KeyValue then 
                    prevIsCheckedOpt <- ValueSome (kvp.Value :?> bool option)
        // Unsubscribe previous event handlers
        let shouldUpdateChecked = not ((identical prevCheckedOpt currCheckedOpt))
        if shouldUpdateChecked then
            match prevCheckedOpt with
            | ValueSome prevValue -> target.Checked.RemoveHandler(prevValue)
            | ValueNone -> ()
        let shouldUpdateUnchecked = not ((identical prevUncheckedOpt currUncheckedOpt))
        if shouldUpdateUnchecked then
            match prevUncheckedOpt with
            | ValueSome prevValue -> target.Unchecked.RemoveHandler(prevValue)
            | ValueNone -> ()
        // Update inherited members
        ViewBuilders.UpdateButtonBase (prevOpt, curr, target)
        // Update properties
        match prevIsCheckedOpt, currIsCheckedOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.IsChecked <- Option.toNullable currValue
        | ValueSome _, ValueNone -> target.IsChecked <- System.Nullable<bool>(true)
        | ValueNone, ValueNone -> ()
        // Subscribe new event handlers
        if shouldUpdateChecked then
            match currCheckedOpt with
            | ValueSome currValue -> target.Checked.AddHandler(currValue)
            | ValueNone -> ()
        if shouldUpdateUnchecked then
            match currUncheckedOpt with
            | ValueSome currValue -> target.Unchecked.AddHandler(currValue)
            | ValueNone -> ()

    /// Builds the attributes for a CheckBox in the view
    static member inline BuildCheckBox(attribCount: int,
                                       ?isChecked: bool option,
                                       ?command: unit -> unit,
                                       ?commandCanExecute: bool,
                                       ?content: ViewElement,
                                       ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                       ?margin: System.Windows.Thickness,
                                       ?verticalAlignment: System.Windows.VerticalAlignment,
                                       ?width: float,
                                       ?checked: unit -> unit,
                                       ?unchecked: unit -> unit) = 
        let attribBuilder = ViewBuilders.BuildToggleButton(attribCount, ?isChecked=isChecked, ?command=command, ?commandCanExecute=commandCanExecute, ?content=content, ?horizontalAlignment=horizontalAlignment, 
                                                           ?margin=margin, ?verticalAlignment=verticalAlignment, ?width=width, ?checked=checked, ?unchecked=unchecked)
        attribBuilder

    static member CreateCheckBox () : System.Windows.Controls.CheckBox =
        new System.Windows.Controls.CheckBox()

    static member UpdateCheckBox (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.CheckBox) = 
        ViewBuilders.UpdateToggleButton (prevOpt, curr, target)

    static member inline ConstructCheckBox(?isChecked: bool option,
                                           ?command: unit -> unit,
                                           ?commandCanExecute: bool,
                                           ?content: ViewElement,
                                           ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                           ?margin: System.Windows.Thickness,
                                           ?verticalAlignment: System.Windows.VerticalAlignment,
                                           ?width: float,
                                           ?checked: unit -> unit,
                                           ?unchecked: unit -> unit) = 

        let attribBuilder = ViewBuilders.BuildCheckBox(0,
                               ?isChecked=isChecked,
                               ?command=command,
                               ?commandCanExecute=commandCanExecute,
                               ?content=content,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width,
                               ?checked=checked,
                               ?unchecked=unchecked)

        ViewElement.Create<System.Windows.Controls.CheckBox>(ViewBuilders.CreateCheckBox, (fun prevOpt curr target -> ViewBuilders.UpdateCheckBox(prevOpt, curr, target)), attribBuilder)

    /// Builds the attributes for a TextBlock in the view
    static member inline BuildTextBlock(attribCount: int,
                                        ?text: string,
                                        ?textAlignment: System.Windows.TextAlignment,
                                        ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                        ?margin: System.Windows.Thickness,
                                        ?verticalAlignment: System.Windows.VerticalAlignment,
                                        ?width: float) = 

        let attribCount = match text with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match textAlignment with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = ViewBuilders.BuildFrameworkElement(attribCount, ?horizontalAlignment=horizontalAlignment, ?margin=margin, ?verticalAlignment=verticalAlignment, ?width=width)
        match text with None -> () | Some v -> attribBuilder.Add(ViewAttributes.TextAttribKey, (v)) 
        match textAlignment with None -> () | Some v -> attribBuilder.Add(ViewAttributes.TextAlignmentAttribKey, (v)) 
        attribBuilder

    static member CreateTextBlock () : System.Windows.Controls.TextBlock =
        new System.Windows.Controls.TextBlock()

    static member UpdateTextBlock (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.TextBlock) = 
        let mutable prevTextOpt = ValueNone
        let mutable currTextOpt = ValueNone
        let mutable prevTextAlignmentOpt = ValueNone
        let mutable currTextAlignmentOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.TextAttribKey.KeyValue then 
                currTextOpt <- ValueSome (kvp.Value :?> string)
            if kvp.Key = ViewAttributes.TextAlignmentAttribKey.KeyValue then 
                currTextAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.TextAlignment)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.TextAttribKey.KeyValue then 
                    prevTextOpt <- ValueSome (kvp.Value :?> string)
                if kvp.Key = ViewAttributes.TextAlignmentAttribKey.KeyValue then 
                    prevTextAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.TextAlignment)
        // Update inherited members
        ViewBuilders.UpdateFrameworkElement (prevOpt, curr, target)
        // Update properties
        match prevTextOpt, currTextOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Text <-  currValue
        | ValueSome _, ValueNone -> target.Text <- System.String.Empty
        | ValueNone, ValueNone -> ()
        match prevTextAlignmentOpt, currTextAlignmentOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.TextAlignment <-  currValue
        | ValueSome _, ValueNone -> target.TextAlignment <- System.Windows.TextAlignment.Left
        | ValueNone, ValueNone -> ()

    static member inline ConstructTextBlock(?text: string,
                                            ?textAlignment: System.Windows.TextAlignment,
                                            ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                            ?margin: System.Windows.Thickness,
                                            ?verticalAlignment: System.Windows.VerticalAlignment,
                                            ?width: float) = 

        let attribBuilder = ViewBuilders.BuildTextBlock(0,
                               ?text=text,
                               ?textAlignment=textAlignment,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

        ViewElement.Create<System.Windows.Controls.TextBlock>(ViewBuilders.CreateTextBlock, (fun prevOpt curr target -> ViewBuilders.UpdateTextBlock(prevOpt, curr, target)), attribBuilder)

    /// Builds the attributes for a RangeBase in the view
    static member inline BuildRangeBase(attribCount: int,
                                        ?minimum: float,
                                        ?maximum: float,
                                        ?value: float,
                                        ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                        ?margin: System.Windows.Thickness,
                                        ?verticalAlignment: System.Windows.VerticalAlignment,
                                        ?width: float,
                                        ?valueChanged: float -> unit) = 

        let attribCount = match minimum with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match maximum with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match value with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match valueChanged with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = ViewBuilders.BuildFrameworkElement(attribCount, ?horizontalAlignment=horizontalAlignment, ?margin=margin, ?verticalAlignment=verticalAlignment, ?width=width)
        match minimum with None -> () | Some v -> attribBuilder.Add(ViewAttributes.MinimumAttribKey, (v)) 
        match maximum with None -> () | Some v -> attribBuilder.Add(ViewAttributes.MaximumAttribKey, (v)) 
        match value with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ValueAttribKey, (v)) 
        match valueChanged with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ValueChangedAttribKey, (fun f -> System.Windows.RoutedPropertyChangedEventHandler<float>(fun _sender _args -> f _args.NewValue))(v)) 
        attribBuilder

    static member UpdateRangeBase (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.Primitives.RangeBase) = 
        let mutable prevValueChangedOpt = ValueNone
        let mutable currValueChangedOpt = ValueNone
        let mutable prevMinimumOpt = ValueNone
        let mutable currMinimumOpt = ValueNone
        let mutable prevMaximumOpt = ValueNone
        let mutable currMaximumOpt = ValueNone
        let mutable prevValueOpt = ValueNone
        let mutable currValueOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.ValueChangedAttribKey.KeyValue then 
                currValueChangedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedPropertyChangedEventHandler<System.Double>)
            if kvp.Key = ViewAttributes.MinimumAttribKey.KeyValue then 
                currMinimumOpt <- ValueSome (kvp.Value :?> float)
            if kvp.Key = ViewAttributes.MaximumAttribKey.KeyValue then 
                currMaximumOpt <- ValueSome (kvp.Value :?> float)
            if kvp.Key = ViewAttributes.ValueAttribKey.KeyValue then 
                currValueOpt <- ValueSome (kvp.Value :?> float)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.ValueChangedAttribKey.KeyValue then 
                    prevValueChangedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedPropertyChangedEventHandler<System.Double>)
                if kvp.Key = ViewAttributes.MinimumAttribKey.KeyValue then 
                    prevMinimumOpt <- ValueSome (kvp.Value :?> float)
                if kvp.Key = ViewAttributes.MaximumAttribKey.KeyValue then 
                    prevMaximumOpt <- ValueSome (kvp.Value :?> float)
                if kvp.Key = ViewAttributes.ValueAttribKey.KeyValue then 
                    prevValueOpt <- ValueSome (kvp.Value :?> float)
        // Unsubscribe previous event handlers
        let shouldUpdateValueChanged = not ((identical prevValueChangedOpt currValueChangedOpt))
        if shouldUpdateValueChanged then
            match prevValueChangedOpt with
            | ValueSome prevValue -> target.ValueChanged.RemoveHandler(prevValue)
            | ValueNone -> ()
        // Update inherited members
        ViewBuilders.UpdateFrameworkElement (prevOpt, curr, target)
        // Update properties
        match prevMinimumOpt, currMinimumOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Minimum <-  currValue
        | ValueSome _, ValueNone -> target.Minimum <- 0.
        | ValueNone, ValueNone -> ()
        match prevMaximumOpt, currMaximumOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Maximum <-  currValue
        | ValueSome _, ValueNone -> target.Maximum <- 1.
        | ValueNone, ValueNone -> ()
        match prevValueOpt, currValueOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Value <-  currValue
        | ValueSome _, ValueNone -> target.Value <- 0.
        | ValueNone, ValueNone -> ()
        // Subscribe new event handlers
        if shouldUpdateValueChanged then
            match currValueChangedOpt with
            | ValueSome currValue -> target.ValueChanged.AddHandler(currValue)
            | ValueNone -> ()

    /// Builds the attributes for a Slider in the view
    static member inline BuildSlider(attribCount: int,
                                     ?minimum: float,
                                     ?maximum: float,
                                     ?value: float,
                                     ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                     ?margin: System.Windows.Thickness,
                                     ?verticalAlignment: System.Windows.VerticalAlignment,
                                     ?width: float,
                                     ?valueChanged: float -> unit) = 
        let attribBuilder = ViewBuilders.BuildRangeBase(attribCount, ?minimum=minimum, ?maximum=maximum, ?value=value, ?horizontalAlignment=horizontalAlignment, ?margin=margin, 
                                                        ?verticalAlignment=verticalAlignment, ?width=width, ?valueChanged=valueChanged)
        attribBuilder

    static member CreateSlider () : System.Windows.Controls.Slider =
        new System.Windows.Controls.Slider()

    static member UpdateSlider (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.Slider) = 
        ViewBuilders.UpdateRangeBase (prevOpt, curr, target)

    static member inline ConstructSlider(?minimum: float,
                                         ?maximum: float,
                                         ?value: float,
                                         ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                         ?margin: System.Windows.Thickness,
                                         ?verticalAlignment: System.Windows.VerticalAlignment,
                                         ?width: float,
                                         ?valueChanged: float -> unit) = 

        let attribBuilder = ViewBuilders.BuildSlider(0,
                               ?minimum=minimum,
                               ?maximum=maximum,
                               ?value=value,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width,
                               ?valueChanged=valueChanged)

        ViewElement.Create<System.Windows.Controls.Slider>(ViewBuilders.CreateSlider, (fun prevOpt curr target -> ViewBuilders.UpdateSlider(prevOpt, curr, target)), attribBuilder)

    /// Builds the attributes for a StackPanel in the view
    static member inline BuildStackPanel(attribCount: int,
                                         ?children: ViewElement list,
                                         ?orientation: System.Windows.Controls.Orientation,
                                         ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                         ?margin: System.Windows.Thickness,
                                         ?verticalAlignment: System.Windows.VerticalAlignment,
                                         ?width: float) = 

        let attribCount = match children with Some _ -> attribCount + 1 | None -> attribCount
        let attribCount = match orientation with Some _ -> attribCount + 1 | None -> attribCount

        let attribBuilder = ViewBuilders.BuildFrameworkElement(attribCount, ?horizontalAlignment=horizontalAlignment, ?margin=margin, ?verticalAlignment=verticalAlignment, ?width=width)
        match children with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ChildrenAttribKey, Array.ofList(v)) 
        match orientation with None -> () | Some v -> attribBuilder.Add(ViewAttributes.OrientationAttribKey, (v)) 
        attribBuilder

    static member CreateStackPanel () : System.Windows.Controls.StackPanel =
        new System.Windows.Controls.StackPanel()

    static member UpdateStackPanel (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.StackPanel) = 
        let mutable prevChildrenOpt = ValueNone
        let mutable currChildrenOpt = ValueNone
        let mutable prevOrientationOpt = ValueNone
        let mutable currOrientationOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.ChildrenAttribKey.KeyValue then 
                currChildrenOpt <- ValueSome (kvp.Value :?> ViewElement array)
            if kvp.Key = ViewAttributes.OrientationAttribKey.KeyValue then 
                currOrientationOpt <- ValueSome (kvp.Value :?> System.Windows.Controls.Orientation)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.ChildrenAttribKey.KeyValue then 
                    prevChildrenOpt <- ValueSome (kvp.Value :?> ViewElement array)
                if kvp.Key = ViewAttributes.OrientationAttribKey.KeyValue then 
                    prevOrientationOpt <- ValueSome (kvp.Value :?> System.Windows.Controls.Orientation)
        // Update inherited members
        ViewBuilders.UpdateFrameworkElement (prevOpt, curr, target)
        // Update properties
        ViewUpdaters.updateStackPanelChildren prevChildrenOpt currChildrenOpt target
        match prevOrientationOpt, currOrientationOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Orientation <-  currValue
        | ValueSome _, ValueNone -> target.Orientation <- System.Windows.Controls.Orientation.Vertical
        | ValueNone, ValueNone -> ()

    static member inline ConstructStackPanel(?children: ViewElement list,
                                             ?orientation: System.Windows.Controls.Orientation,
                                             ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                             ?margin: System.Windows.Thickness,
                                             ?verticalAlignment: System.Windows.VerticalAlignment,
                                             ?width: float) = 

        let attribBuilder = ViewBuilders.BuildStackPanel(0,
                               ?children=children,
                               ?orientation=orientation,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

        ViewElement.Create<System.Windows.Controls.StackPanel>(ViewBuilders.CreateStackPanel, (fun prevOpt curr target -> ViewBuilders.UpdateStackPanel(prevOpt, curr, target)), attribBuilder)

/// Viewer that allows to read the properties of a ViewElement representing a FrameworkElement
type FrameworkElementViewer(element: ViewElement) =
    do if not ((typeof<System.Windows.FrameworkElement>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.FrameworkElement' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the HorizontalAlignment member
    member this.HorizontalAlignment = element.GetAttributeKeyed(ViewAttributes.HorizontalAlignmentAttribKey)
    /// Get the value of the Margin member
    member this.Margin = element.GetAttributeKeyed(ViewAttributes.MarginAttribKey)
    /// Get the value of the VerticalAlignment member
    member this.VerticalAlignment = element.GetAttributeKeyed(ViewAttributes.VerticalAlignmentAttribKey)
    /// Get the value of the Width member
    member this.Width = element.GetAttributeKeyed(ViewAttributes.WidthAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a ContentControl
type ContentControlViewer(element: ViewElement) =
    inherit FrameworkElementViewer(element)
    do if not ((typeof<System.Windows.Controls.ContentControl>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.ContentControl' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Content member
    member this.Content = element.GetAttributeKeyed(ViewAttributes.ContentControlContentAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a Window
type WindowViewer(element: ViewElement) =
    inherit ContentControlViewer(element)
    do if not ((typeof<System.Windows.Window>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Window' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Title member
    member this.Title = element.GetAttributeKeyed(ViewAttributes.TitleAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a ButtonBase
type ButtonBaseViewer(element: ViewElement) =
    inherit ContentControlViewer(element)
    do if not ((typeof<System.Windows.Controls.Primitives.ButtonBase>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.Primitives.ButtonBase' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Command member
    member this.Command = element.GetAttributeKeyed(ViewAttributes.CommandAttribKey)
    /// Get the value of the CommandCanExecute member
    member this.CommandCanExecute = element.GetAttributeKeyed(ViewAttributes.CommandCanExecuteAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a Button
type ButtonViewer(element: ViewElement) =
    inherit ButtonBaseViewer(element)
    do if not ((typeof<System.Windows.Controls.Button>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.Button' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Content member
    member this.Content = element.GetAttributeKeyed(ViewAttributes.ButtonContentAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a ToggleButton
type ToggleButtonViewer(element: ViewElement) =
    inherit ButtonBaseViewer(element)
    do if not ((typeof<System.Windows.Controls.Primitives.ToggleButton>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.Primitives.ToggleButton' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the IsChecked member
    member this.IsChecked = element.GetAttributeKeyed(ViewAttributes.IsCheckedAttribKey)
    /// Get the value of the Checked member
    member this.Checked = element.GetAttributeKeyed(ViewAttributes.CheckedAttribKey)
    /// Get the value of the Unchecked member
    member this.Unchecked = element.GetAttributeKeyed(ViewAttributes.UncheckedAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a CheckBox
type CheckBoxViewer(element: ViewElement) =
    inherit ToggleButtonViewer(element)
    do if not ((typeof<System.Windows.Controls.CheckBox>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.CheckBox' is expected, but '%s' was provided." element.TargetType.FullName

/// Viewer that allows to read the properties of a ViewElement representing a TextBlock
type TextBlockViewer(element: ViewElement) =
    inherit FrameworkElementViewer(element)
    do if not ((typeof<System.Windows.Controls.TextBlock>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.TextBlock' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Text member
    member this.Text = element.GetAttributeKeyed(ViewAttributes.TextAttribKey)
    /// Get the value of the TextAlignment member
    member this.TextAlignment = element.GetAttributeKeyed(ViewAttributes.TextAlignmentAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a RangeBase
type RangeBaseViewer(element: ViewElement) =
    inherit FrameworkElementViewer(element)
    do if not ((typeof<System.Windows.Controls.Primitives.RangeBase>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.Primitives.RangeBase' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Minimum member
    member this.Minimum = element.GetAttributeKeyed(ViewAttributes.MinimumAttribKey)
    /// Get the value of the Maximum member
    member this.Maximum = element.GetAttributeKeyed(ViewAttributes.MaximumAttribKey)
    /// Get the value of the Value member
    member this.Value = element.GetAttributeKeyed(ViewAttributes.ValueAttribKey)
    /// Get the value of the ValueChanged member
    member this.ValueChanged = element.GetAttributeKeyed(ViewAttributes.ValueChangedAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a Slider
type SliderViewer(element: ViewElement) =
    inherit RangeBaseViewer(element)
    do if not ((typeof<System.Windows.Controls.Slider>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.Slider' is expected, but '%s' was provided." element.TargetType.FullName

/// Viewer that allows to read the properties of a ViewElement representing a StackPanel
type StackPanelViewer(element: ViewElement) =
    inherit FrameworkElementViewer(element)
    do if not ((typeof<System.Windows.Controls.StackPanel>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.StackPanel' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Children member
    member this.Children = element.GetAttributeKeyed(ViewAttributes.ChildrenAttribKey)
    /// Get the value of the Orientation member
    member this.Orientation = element.GetAttributeKeyed(ViewAttributes.OrientationAttribKey)

[<AbstractClass; Sealed>]
type View private () =
    /// Describes a ContentControl in the view
    static member inline ContentControl(?content: ViewElement,
                                        ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                        ?margin: System.Windows.Thickness,
                                        ?verticalAlignment: System.Windows.VerticalAlignment,
                                        ?width: float) =

        ViewBuilders.ConstructContentControl(?content=content,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

    /// Describes a Window in the view
    static member inline Window(?content: ViewElement,
                                ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                ?margin: System.Windows.Thickness,
                                ?title: string,
                                ?verticalAlignment: System.Windows.VerticalAlignment,
                                ?width: float) =

        ViewBuilders.ConstructWindow(?content=content,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?title=title,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

    /// Describes a Button in the view
    static member inline Button(?command: unit -> unit,
                                ?commandCanExecute: bool,
                                ?content: string,
                                ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                ?margin: System.Windows.Thickness,
                                ?verticalAlignment: System.Windows.VerticalAlignment,
                                ?width: float) =

        ViewBuilders.ConstructButton(?command=command,
                               ?commandCanExecute=commandCanExecute,
                               ?content=content,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

    /// Describes a CheckBox in the view
    static member inline CheckBox(?checked: unit -> unit,
                                  ?command: unit -> unit,
                                  ?commandCanExecute: bool,
                                  ?content: ViewElement,
                                  ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                  ?isChecked: bool option,
                                  ?margin: System.Windows.Thickness,
                                  ?unchecked: unit -> unit,
                                  ?verticalAlignment: System.Windows.VerticalAlignment,
                                  ?width: float) =

        ViewBuilders.ConstructCheckBox(?checked=checked,
                               ?command=command,
                               ?commandCanExecute=commandCanExecute,
                               ?content=content,
                               ?horizontalAlignment=horizontalAlignment,
                               ?isChecked=isChecked,
                               ?margin=margin,
                               ?unchecked=unchecked,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

    /// Describes a TextBlock in the view
    static member inline TextBlock(?text: string,
                                   ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                   ?margin: System.Windows.Thickness,
                                   ?textAlignment: System.Windows.TextAlignment,
                                   ?verticalAlignment: System.Windows.VerticalAlignment,
                                   ?width: float) =

        ViewBuilders.ConstructTextBlock(?text=text,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?textAlignment=textAlignment,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

    /// Describes a Slider in the view
    static member inline Slider(?horizontalAlignment: System.Windows.HorizontalAlignment,
                                ?margin: System.Windows.Thickness,
                                ?maximum: float,
                                ?minimum: float,
                                ?value: float,
                                ?valueChanged: float -> unit,
                                ?verticalAlignment: System.Windows.VerticalAlignment,
                                ?width: float) =

        ViewBuilders.ConstructSlider(?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?maximum=maximum,
                               ?minimum=minimum,
                               ?value=value,
                               ?valueChanged=valueChanged,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)

    /// Describes a StackPanel in the view
    static member inline StackPanel(?children: ViewElement list,
                                    ?horizontalAlignment: System.Windows.HorizontalAlignment,
                                    ?margin: System.Windows.Thickness,
                                    ?orientation: System.Windows.Controls.Orientation,
                                    ?verticalAlignment: System.Windows.VerticalAlignment,
                                    ?width: float) =

        ViewBuilders.ConstructStackPanel(?children=children,
                               ?horizontalAlignment=horizontalAlignment,
                               ?margin=margin,
                               ?orientation=orientation,
                               ?verticalAlignment=verticalAlignment,
                               ?width=width)


[<AutoOpen>]
module ViewElementExtensions = 

    type ViewElement with

        /// Adjusts the HorizontalAlignment property in the visual element
        member x.HorizontalAlignment(value: System.Windows.HorizontalAlignment) = x.WithAttribute(ViewAttributes.HorizontalAlignmentAttribKey, (value))

        /// Adjusts the Margin property in the visual element
        member x.Margin(value: System.Windows.Thickness) = x.WithAttribute(ViewAttributes.MarginAttribKey, (value))

        /// Adjusts the VerticalAlignment property in the visual element
        member x.VerticalAlignment(value: System.Windows.VerticalAlignment) = x.WithAttribute(ViewAttributes.VerticalAlignmentAttribKey, (value))

        /// Adjusts the Width property in the visual element
        member x.Width(value: float) = x.WithAttribute(ViewAttributes.WidthAttribKey, (value))

        /// Adjusts the ContentControlContent property in the visual element
        member x.ContentControlContent(value: ViewElement) = x.WithAttribute(ViewAttributes.ContentControlContentAttribKey, (value))

        /// Adjusts the Title property in the visual element
        member x.Title(value: string) = x.WithAttribute(ViewAttributes.TitleAttribKey, (value))

        /// Adjusts the Command property in the visual element
        member x.Command(value: unit -> unit) = x.WithAttribute(ViewAttributes.CommandAttribKey, (value))

        /// Adjusts the CommandCanExecute property in the visual element
        member x.CommandCanExecute(value: bool) = x.WithAttribute(ViewAttributes.CommandCanExecuteAttribKey, (value))

        /// Adjusts the ButtonContent property in the visual element
        member x.ButtonContent(value: string) = x.WithAttribute(ViewAttributes.ButtonContentAttribKey, (value))

        /// Adjusts the Checked property in the visual element
        member x.Checked(value: unit -> unit) = x.WithAttribute(ViewAttributes.CheckedAttribKey, (fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))(value))

        /// Adjusts the Unchecked property in the visual element
        member x.Unchecked(value: unit -> unit) = x.WithAttribute(ViewAttributes.UncheckedAttribKey, (fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))(value))

        /// Adjusts the IsChecked property in the visual element
        member x.IsChecked(value: bool option) = x.WithAttribute(ViewAttributes.IsCheckedAttribKey, (value))

        /// Adjusts the Text property in the visual element
        member x.Text(value: string) = x.WithAttribute(ViewAttributes.TextAttribKey, (value))

        /// Adjusts the TextAlignment property in the visual element
        member x.TextAlignment(value: System.Windows.TextAlignment) = x.WithAttribute(ViewAttributes.TextAlignmentAttribKey, (value))

        /// Adjusts the ValueChanged property in the visual element
        member x.ValueChanged(value: float -> unit) = x.WithAttribute(ViewAttributes.ValueChangedAttribKey, (fun f -> System.Windows.RoutedPropertyChangedEventHandler<float>(fun _sender _args -> f _args.NewValue))(value))

        /// Adjusts the Minimum property in the visual element
        member x.Minimum(value: float) = x.WithAttribute(ViewAttributes.MinimumAttribKey, (value))

        /// Adjusts the Maximum property in the visual element
        member x.Maximum(value: float) = x.WithAttribute(ViewAttributes.MaximumAttribKey, (value))

        /// Adjusts the Value property in the visual element
        member x.Value(value: float) = x.WithAttribute(ViewAttributes.ValueAttribKey, (value))

        /// Adjusts the Children property in the visual element
        member x.Children(value: ViewElement list) = x.WithAttribute(ViewAttributes.ChildrenAttribKey, Array.ofList(value))

        /// Adjusts the Orientation property in the visual element
        member x.Orientation(value: System.Windows.Controls.Orientation) = x.WithAttribute(ViewAttributes.OrientationAttribKey, (value))

        member inline x.With(?horizontalAlignment: System.Windows.HorizontalAlignment, ?margin: System.Windows.Thickness, ?verticalAlignment: System.Windows.VerticalAlignment, ?width: float, ?contentControlContent: ViewElement, 
                             ?title: string, ?command: unit -> unit, ?commandCanExecute: bool, ?buttonContent: string, ?checked: unit -> unit, 
                             ?unchecked: unit -> unit, ?isChecked: bool option, ?text: string, ?textAlignment: System.Windows.TextAlignment, ?valueChanged: float -> unit, 
                             ?minimum: float, ?maximum: float, ?value: float, ?children: ViewElement list, ?orientation: System.Windows.Controls.Orientation) =
            let x = match horizontalAlignment with None -> x | Some opt -> x.HorizontalAlignment(opt)
            let x = match margin with None -> x | Some opt -> x.Margin(opt)
            let x = match verticalAlignment with None -> x | Some opt -> x.VerticalAlignment(opt)
            let x = match width with None -> x | Some opt -> x.Width(opt)
            let x = match contentControlContent with None -> x | Some opt -> x.ContentControlContent(opt)
            let x = match title with None -> x | Some opt -> x.Title(opt)
            let x = match command with None -> x | Some opt -> x.Command(opt)
            let x = match commandCanExecute with None -> x | Some opt -> x.CommandCanExecute(opt)
            let x = match buttonContent with None -> x | Some opt -> x.ButtonContent(opt)
            let x = match checked with None -> x | Some opt -> x.Checked(opt)
            let x = match unchecked with None -> x | Some opt -> x.Unchecked(opt)
            let x = match isChecked with None -> x | Some opt -> x.IsChecked(opt)
            let x = match text with None -> x | Some opt -> x.Text(opt)
            let x = match textAlignment with None -> x | Some opt -> x.TextAlignment(opt)
            let x = match valueChanged with None -> x | Some opt -> x.ValueChanged(opt)
            let x = match minimum with None -> x | Some opt -> x.Minimum(opt)
            let x = match maximum with None -> x | Some opt -> x.Maximum(opt)
            let x = match value with None -> x | Some opt -> x.Value(opt)
            let x = match children with None -> x | Some opt -> x.Children(opt)
            let x = match orientation with None -> x | Some opt -> x.Orientation(opt)
            x

    /// Adjusts the HorizontalAlignment property in the visual element
    let horizontalAlignment (value: System.Windows.HorizontalAlignment) (x: ViewElement) = x.HorizontalAlignment(value)
    /// Adjusts the Margin property in the visual element
    let margin (value: System.Windows.Thickness) (x: ViewElement) = x.Margin(value)
    /// Adjusts the VerticalAlignment property in the visual element
    let verticalAlignment (value: System.Windows.VerticalAlignment) (x: ViewElement) = x.VerticalAlignment(value)
    /// Adjusts the Width property in the visual element
    let width (value: float) (x: ViewElement) = x.Width(value)
    /// Adjusts the ContentControlContent property in the visual element
    let contentControlContent (value: ViewElement) (x: ViewElement) = x.ContentControlContent(value)
    /// Adjusts the Title property in the visual element
    let title (value: string) (x: ViewElement) = x.Title(value)
    /// Adjusts the Command property in the visual element
    let command (value: unit -> unit) (x: ViewElement) = x.Command(value)
    /// Adjusts the CommandCanExecute property in the visual element
    let commandCanExecute (value: bool) (x: ViewElement) = x.CommandCanExecute(value)
    /// Adjusts the ButtonContent property in the visual element
    let buttonContent (value: string) (x: ViewElement) = x.ButtonContent(value)
    /// Adjusts the Checked property in the visual element
    let checked (value: unit -> unit) (x: ViewElement) = x.Checked(value)
    /// Adjusts the Unchecked property in the visual element
    let unchecked (value: unit -> unit) (x: ViewElement) = x.Unchecked(value)
    /// Adjusts the IsChecked property in the visual element
    let isChecked (value: bool option) (x: ViewElement) = x.IsChecked(value)
    /// Adjusts the Text property in the visual element
    let text (value: string) (x: ViewElement) = x.Text(value)
    /// Adjusts the TextAlignment property in the visual element
    let textAlignment (value: System.Windows.TextAlignment) (x: ViewElement) = x.TextAlignment(value)
    /// Adjusts the ValueChanged property in the visual element
    let valueChanged (value: float -> unit) (x: ViewElement) = x.ValueChanged(value)
    /// Adjusts the Minimum property in the visual element
    let minimum (value: float) (x: ViewElement) = x.Minimum(value)
    /// Adjusts the Maximum property in the visual element
    let maximum (value: float) (x: ViewElement) = x.Maximum(value)
    /// Adjusts the Value property in the visual element
    let value (value: float) (x: ViewElement) = x.Value(value)
    /// Adjusts the Children property in the visual element
    let children (value: ViewElement list) (x: ViewElement) = x.Children(value)
    /// Adjusts the Orientation property in the visual element
    let orientation (value: System.Windows.Controls.Orientation) (x: ViewElement) = x.Orientation(value)

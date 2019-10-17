// Copyright 2018-2019 Fabulous contributors. See LICENSE.md for license.
namespace Fabulous.WPF

#nowarn "59" // cast always holds
#nowarn "66" // cast always holds
#nowarn "67" // cast always holds

open Fabulous

module ViewAttributes =
    let FrameworkElementHorizontalAlignmentAttribKey : AttributeKey<_> = AttributeKey<_>("FrameworkElementHorizontalAlignment")
    let FrameworkElementMarginAttribKey : AttributeKey<_> = AttributeKey<_>("FrameworkElementMargin")
    let FrameworkElementVerticalAlignmentAttribKey : AttributeKey<_> = AttributeKey<_>("FrameworkElementVerticalAlignment")
    let FrameworkElementWidthAttribKey : AttributeKey<_> = AttributeKey<_>("FrameworkElementWidth")
    let ContentControlContentAttribKey : AttributeKey<_> = AttributeKey<_>("ContentControlContent")
    let WindowTitleAttribKey : AttributeKey<_> = AttributeKey<_>("WindowTitle")
    let ButtonBaseCommandAttribKey : AttributeKey<_> = AttributeKey<_>("ButtonBaseCommand")
    let ButtonBaseCommandCanExecuteAttribKey : AttributeKey<_> = AttributeKey<_>("ButtonBaseCommandCanExecute")
    let ButtonContentAttribKey : AttributeKey<_> = AttributeKey<_>("ButtonContent")
    let ToggleButtonCheckedAttribKey : AttributeKey<_> = AttributeKey<_>("ToggleButtonChecked")
    let ToggleButtonUncheckedAttribKey : AttributeKey<_> = AttributeKey<_>("ToggleButtonUnchecked")
    let ToggleButtonIsCheckedAttribKey : AttributeKey<_> = AttributeKey<_>("ToggleButtonIsChecked")
    let TextBlockTextAttribKey : AttributeKey<_> = AttributeKey<_>("TextBlockText")
    let TextBlockTextAlignmentAttribKey : AttributeKey<_> = AttributeKey<_>("TextBlockTextAlignment")
    let RangeBaseValueChangedAttribKey : AttributeKey<_> = AttributeKey<_>("RangeBaseValueChanged")
    let RangeBaseMinimumAttribKey : AttributeKey<_> = AttributeKey<_>("RangeBaseMinimum")
    let RangeBaseMaximumAttribKey : AttributeKey<_> = AttributeKey<_>("RangeBaseMaximum")
    let RangeBaseValueAttribKey : AttributeKey<_> = AttributeKey<_>("RangeBaseValue")
    let StackPanelChildrenAttribKey : AttributeKey<_> = AttributeKey<_>("StackPanelChildren")
    let StackPanelOrientationAttribKey : AttributeKey<_> = AttributeKey<_>("StackPanelOrientation")

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
        match horizontalAlignment with None -> () | Some v -> attribBuilder.Add(ViewAttributes.FrameworkElementHorizontalAlignmentAttribKey, (v)) 
        match margin with None -> () | Some v -> attribBuilder.Add(ViewAttributes.FrameworkElementMarginAttribKey, (v)) 
        match verticalAlignment with None -> () | Some v -> attribBuilder.Add(ViewAttributes.FrameworkElementVerticalAlignmentAttribKey, (v)) 
        match width with None -> () | Some v -> attribBuilder.Add(ViewAttributes.FrameworkElementWidthAttribKey, (v)) 
        attribBuilder

    static member UpdateFrameworkElement (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.FrameworkElement) = 
        let mutable prevFrameworkElementHorizontalAlignmentOpt = ValueNone
        let mutable currFrameworkElementHorizontalAlignmentOpt = ValueNone
        let mutable prevFrameworkElementMarginOpt = ValueNone
        let mutable currFrameworkElementMarginOpt = ValueNone
        let mutable prevFrameworkElementVerticalAlignmentOpt = ValueNone
        let mutable currFrameworkElementVerticalAlignmentOpt = ValueNone
        let mutable prevFrameworkElementWidthOpt = ValueNone
        let mutable currFrameworkElementWidthOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.FrameworkElementHorizontalAlignmentAttribKey.KeyValue then 
                currFrameworkElementHorizontalAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.HorizontalAlignment)
            if kvp.Key = ViewAttributes.FrameworkElementMarginAttribKey.KeyValue then 
                currFrameworkElementMarginOpt <- ValueSome (kvp.Value :?> System.Windows.Thickness)
            if kvp.Key = ViewAttributes.FrameworkElementVerticalAlignmentAttribKey.KeyValue then 
                currFrameworkElementVerticalAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.VerticalAlignment)
            if kvp.Key = ViewAttributes.FrameworkElementWidthAttribKey.KeyValue then 
                currFrameworkElementWidthOpt <- ValueSome (kvp.Value :?> float)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.FrameworkElementHorizontalAlignmentAttribKey.KeyValue then 
                    prevFrameworkElementHorizontalAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.HorizontalAlignment)
                if kvp.Key = ViewAttributes.FrameworkElementMarginAttribKey.KeyValue then 
                    prevFrameworkElementMarginOpt <- ValueSome (kvp.Value :?> System.Windows.Thickness)
                if kvp.Key = ViewAttributes.FrameworkElementVerticalAlignmentAttribKey.KeyValue then 
                    prevFrameworkElementVerticalAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.VerticalAlignment)
                if kvp.Key = ViewAttributes.FrameworkElementWidthAttribKey.KeyValue then 
                    prevFrameworkElementWidthOpt <- ValueSome (kvp.Value :?> float)
        // Update properties
        match prevFrameworkElementHorizontalAlignmentOpt, currFrameworkElementHorizontalAlignmentOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.HorizontalAlignment <-  currValue
        | ValueSome _, ValueNone -> target.HorizontalAlignment <- System.Windows.HorizontalAlignment.Stretch
        | ValueNone, ValueNone -> ()
        match prevFrameworkElementMarginOpt, currFrameworkElementMarginOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Margin <-  currValue
        | ValueSome _, ValueNone -> target.Margin <- System.Windows.Thickness(0.)
        | ValueNone, ValueNone -> ()
        match prevFrameworkElementVerticalAlignmentOpt, currFrameworkElementVerticalAlignmentOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.VerticalAlignment <-  currValue
        | ValueSome _, ValueNone -> target.VerticalAlignment <- System.Windows.VerticalAlignment.Stretch
        | ValueNone, ValueNone -> ()
        match prevFrameworkElementWidthOpt, currFrameworkElementWidthOpt with
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
        match title with None -> () | Some v -> attribBuilder.Add(ViewAttributes.WindowTitleAttribKey, (v)) 
        attribBuilder

    static member CreateWindow () : System.Windows.Window =
        new System.Windows.Window()

    static member UpdateWindow (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Window) = 
        let mutable prevWindowTitleOpt = ValueNone
        let mutable currWindowTitleOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.WindowTitleAttribKey.KeyValue then 
                currWindowTitleOpt <- ValueSome (kvp.Value :?> string)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.WindowTitleAttribKey.KeyValue then 
                    prevWindowTitleOpt <- ValueSome (kvp.Value :?> string)
        // Update inherited members
        ViewBuilders.UpdateContentControl (prevOpt, curr, target)
        // Update properties
        match prevWindowTitleOpt, currWindowTitleOpt with
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
        match command with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ButtonBaseCommandAttribKey, (v)) 
        match commandCanExecute with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ButtonBaseCommandCanExecuteAttribKey, (v)) 
        attribBuilder

    static member UpdateButtonBase (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.Primitives.ButtonBase) = 
        let mutable prevButtonBaseCommandOpt = ValueNone
        let mutable currButtonBaseCommandOpt = ValueNone
        let mutable prevButtonBaseCommandCanExecuteOpt = ValueNone
        let mutable currButtonBaseCommandCanExecuteOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.ButtonBaseCommandAttribKey.KeyValue then 
                currButtonBaseCommandOpt <- ValueSome (kvp.Value :?> unit -> unit)
            if kvp.Key = ViewAttributes.ButtonBaseCommandCanExecuteAttribKey.KeyValue then 
                currButtonBaseCommandCanExecuteOpt <- ValueSome (kvp.Value :?> bool)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.ButtonBaseCommandAttribKey.KeyValue then 
                    prevButtonBaseCommandOpt <- ValueSome (kvp.Value :?> unit -> unit)
                if kvp.Key = ViewAttributes.ButtonBaseCommandCanExecuteAttribKey.KeyValue then 
                    prevButtonBaseCommandCanExecuteOpt <- ValueSome (kvp.Value :?> bool)
        // Update inherited members
        ViewBuilders.UpdateContentControl (prevOpt, curr, target)
        // Update properties
        (fun _ _ _ -> ()) prevButtonBaseCommandOpt currButtonBaseCommandOpt target
        ViewUpdaters.updateCommand prevButtonBaseCommandOpt currButtonBaseCommandOpt (fun _target -> ()) (fun (target: System.Windows.Controls.Primitives.ButtonBase) cmd -> target.Command <- cmd) prevButtonBaseCommandCanExecuteOpt currButtonBaseCommandCanExecuteOpt target

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
        match isChecked with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ToggleButtonIsCheckedAttribKey, (v)) 
        match checked with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ToggleButtonCheckedAttribKey, (fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))(v)) 
        match unchecked with None -> () | Some v -> attribBuilder.Add(ViewAttributes.ToggleButtonUncheckedAttribKey, (fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))(v)) 
        attribBuilder

    static member UpdateToggleButton (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.Primitives.ToggleButton) = 
        let mutable prevToggleButtonCheckedOpt = ValueNone
        let mutable currToggleButtonCheckedOpt = ValueNone
        let mutable prevToggleButtonUncheckedOpt = ValueNone
        let mutable currToggleButtonUncheckedOpt = ValueNone
        let mutable prevToggleButtonIsCheckedOpt = ValueNone
        let mutable currToggleButtonIsCheckedOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.ToggleButtonCheckedAttribKey.KeyValue then 
                currToggleButtonCheckedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedEventHandler)
            if kvp.Key = ViewAttributes.ToggleButtonUncheckedAttribKey.KeyValue then 
                currToggleButtonUncheckedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedEventHandler)
            if kvp.Key = ViewAttributes.ToggleButtonIsCheckedAttribKey.KeyValue then 
                currToggleButtonIsCheckedOpt <- ValueSome (kvp.Value :?> bool option)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.ToggleButtonCheckedAttribKey.KeyValue then 
                    prevToggleButtonCheckedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedEventHandler)
                if kvp.Key = ViewAttributes.ToggleButtonUncheckedAttribKey.KeyValue then 
                    prevToggleButtonUncheckedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedEventHandler)
                if kvp.Key = ViewAttributes.ToggleButtonIsCheckedAttribKey.KeyValue then 
                    prevToggleButtonIsCheckedOpt <- ValueSome (kvp.Value :?> bool option)
        // Unsubscribe previous event handlers
        let shouldUpdateToggleButtonChecked = not ((identical prevToggleButtonCheckedOpt currToggleButtonCheckedOpt))
        if shouldUpdateToggleButtonChecked then
            match prevToggleButtonCheckedOpt with
            | ValueSome prevValue -> target.Checked.RemoveHandler(prevValue)
            | ValueNone -> ()
        let shouldUpdateToggleButtonUnchecked = not ((identical prevToggleButtonUncheckedOpt currToggleButtonUncheckedOpt))
        if shouldUpdateToggleButtonUnchecked then
            match prevToggleButtonUncheckedOpt with
            | ValueSome prevValue -> target.Unchecked.RemoveHandler(prevValue)
            | ValueNone -> ()
        // Update inherited members
        ViewBuilders.UpdateButtonBase (prevOpt, curr, target)
        // Update properties
        match prevToggleButtonIsCheckedOpt, currToggleButtonIsCheckedOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.IsChecked <- Option.toNullable currValue
        | ValueSome _, ValueNone -> target.IsChecked <- System.Nullable<bool>(true)
        | ValueNone, ValueNone -> ()
        // Subscribe new event handlers
        if shouldUpdateToggleButtonChecked then
            match currToggleButtonCheckedOpt with
            | ValueSome currValue -> target.Checked.AddHandler(currValue)
            | ValueNone -> ()
        if shouldUpdateToggleButtonUnchecked then
            match currToggleButtonUncheckedOpt with
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
        match text with None -> () | Some v -> attribBuilder.Add(ViewAttributes.TextBlockTextAttribKey, (v)) 
        match textAlignment with None -> () | Some v -> attribBuilder.Add(ViewAttributes.TextBlockTextAlignmentAttribKey, (v)) 
        attribBuilder

    static member CreateTextBlock () : System.Windows.Controls.TextBlock =
        new System.Windows.Controls.TextBlock()

    static member UpdateTextBlock (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.TextBlock) = 
        let mutable prevTextBlockTextOpt = ValueNone
        let mutable currTextBlockTextOpt = ValueNone
        let mutable prevTextBlockTextAlignmentOpt = ValueNone
        let mutable currTextBlockTextAlignmentOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.TextBlockTextAttribKey.KeyValue then 
                currTextBlockTextOpt <- ValueSome (kvp.Value :?> string)
            if kvp.Key = ViewAttributes.TextBlockTextAlignmentAttribKey.KeyValue then 
                currTextBlockTextAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.TextAlignment)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.TextBlockTextAttribKey.KeyValue then 
                    prevTextBlockTextOpt <- ValueSome (kvp.Value :?> string)
                if kvp.Key = ViewAttributes.TextBlockTextAlignmentAttribKey.KeyValue then 
                    prevTextBlockTextAlignmentOpt <- ValueSome (kvp.Value :?> System.Windows.TextAlignment)
        // Update inherited members
        ViewBuilders.UpdateFrameworkElement (prevOpt, curr, target)
        // Update properties
        match prevTextBlockTextOpt, currTextBlockTextOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Text <-  currValue
        | ValueSome _, ValueNone -> target.Text <- System.String.Empty
        | ValueNone, ValueNone -> ()
        match prevTextBlockTextAlignmentOpt, currTextBlockTextAlignmentOpt with
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
        match minimum with None -> () | Some v -> attribBuilder.Add(ViewAttributes.RangeBaseMinimumAttribKey, (v)) 
        match maximum with None -> () | Some v -> attribBuilder.Add(ViewAttributes.RangeBaseMaximumAttribKey, (v)) 
        match value with None -> () | Some v -> attribBuilder.Add(ViewAttributes.RangeBaseValueAttribKey, (v)) 
        match valueChanged with None -> () | Some v -> attribBuilder.Add(ViewAttributes.RangeBaseValueChangedAttribKey, (fun f -> System.Windows.RoutedPropertyChangedEventHandler<float>(fun _sender _args -> f _args.NewValue))(v)) 
        attribBuilder

    static member UpdateRangeBase (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.Primitives.RangeBase) = 
        let mutable prevRangeBaseValueChangedOpt = ValueNone
        let mutable currRangeBaseValueChangedOpt = ValueNone
        let mutable prevRangeBaseMinimumOpt = ValueNone
        let mutable currRangeBaseMinimumOpt = ValueNone
        let mutable prevRangeBaseMaximumOpt = ValueNone
        let mutable currRangeBaseMaximumOpt = ValueNone
        let mutable prevRangeBaseValueOpt = ValueNone
        let mutable currRangeBaseValueOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.RangeBaseValueChangedAttribKey.KeyValue then 
                currRangeBaseValueChangedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedPropertyChangedEventHandler<System.Double>)
            if kvp.Key = ViewAttributes.RangeBaseMinimumAttribKey.KeyValue then 
                currRangeBaseMinimumOpt <- ValueSome (kvp.Value :?> float)
            if kvp.Key = ViewAttributes.RangeBaseMaximumAttribKey.KeyValue then 
                currRangeBaseMaximumOpt <- ValueSome (kvp.Value :?> float)
            if kvp.Key = ViewAttributes.RangeBaseValueAttribKey.KeyValue then 
                currRangeBaseValueOpt <- ValueSome (kvp.Value :?> float)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.RangeBaseValueChangedAttribKey.KeyValue then 
                    prevRangeBaseValueChangedOpt <- ValueSome (kvp.Value :?> System.Windows.RoutedPropertyChangedEventHandler<System.Double>)
                if kvp.Key = ViewAttributes.RangeBaseMinimumAttribKey.KeyValue then 
                    prevRangeBaseMinimumOpt <- ValueSome (kvp.Value :?> float)
                if kvp.Key = ViewAttributes.RangeBaseMaximumAttribKey.KeyValue then 
                    prevRangeBaseMaximumOpt <- ValueSome (kvp.Value :?> float)
                if kvp.Key = ViewAttributes.RangeBaseValueAttribKey.KeyValue then 
                    prevRangeBaseValueOpt <- ValueSome (kvp.Value :?> float)
        // Unsubscribe previous event handlers
        let shouldUpdateRangeBaseValueChanged = not ((identical prevRangeBaseValueChangedOpt currRangeBaseValueChangedOpt))
        if shouldUpdateRangeBaseValueChanged then
            match prevRangeBaseValueChangedOpt with
            | ValueSome prevValue -> target.ValueChanged.RemoveHandler(prevValue)
            | ValueNone -> ()
        // Update inherited members
        ViewBuilders.UpdateFrameworkElement (prevOpt, curr, target)
        // Update properties
        match prevRangeBaseMinimumOpt, currRangeBaseMinimumOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Minimum <-  currValue
        | ValueSome _, ValueNone -> target.Minimum <- 0.
        | ValueNone, ValueNone -> ()
        match prevRangeBaseMaximumOpt, currRangeBaseMaximumOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Maximum <-  currValue
        | ValueSome _, ValueNone -> target.Maximum <- 1.
        | ValueNone, ValueNone -> ()
        match prevRangeBaseValueOpt, currRangeBaseValueOpt with
        | ValueSome prevValue, ValueSome currValue when prevValue = currValue -> ()
        | _, ValueSome currValue -> target.Value <-  currValue
        | ValueSome _, ValueNone -> target.Value <- 0.
        | ValueNone, ValueNone -> ()
        // Subscribe new event handlers
        if shouldUpdateRangeBaseValueChanged then
            match currRangeBaseValueChangedOpt with
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
        match children with None -> () | Some v -> attribBuilder.Add(ViewAttributes.StackPanelChildrenAttribKey, Array.ofList(v)) 
        match orientation with None -> () | Some v -> attribBuilder.Add(ViewAttributes.StackPanelOrientationAttribKey, (v)) 
        attribBuilder

    static member CreateStackPanel () : System.Windows.Controls.StackPanel =
        new System.Windows.Controls.StackPanel()

    static member UpdateStackPanel (prevOpt: ViewElement voption, curr: ViewElement, target: System.Windows.Controls.StackPanel) = 
        let mutable prevStackPanelChildrenOpt = ValueNone
        let mutable currStackPanelChildrenOpt = ValueNone
        let mutable prevStackPanelOrientationOpt = ValueNone
        let mutable currStackPanelOrientationOpt = ValueNone
        for kvp in curr.AttributesKeyed do
            if kvp.Key = ViewAttributes.StackPanelChildrenAttribKey.KeyValue then 
                currStackPanelChildrenOpt <- ValueSome (kvp.Value :?> ViewElement array)
            if kvp.Key = ViewAttributes.StackPanelOrientationAttribKey.KeyValue then 
                currStackPanelOrientationOpt <- ValueSome (kvp.Value :?> System.Windows.Controls.Orientation)
        match prevOpt with
        | ValueNone -> ()
        | ValueSome prev ->
            for kvp in prev.AttributesKeyed do
                if kvp.Key = ViewAttributes.StackPanelChildrenAttribKey.KeyValue then 
                    prevStackPanelChildrenOpt <- ValueSome (kvp.Value :?> ViewElement array)
                if kvp.Key = ViewAttributes.StackPanelOrientationAttribKey.KeyValue then 
                    prevStackPanelOrientationOpt <- ValueSome (kvp.Value :?> System.Windows.Controls.Orientation)
        // Update inherited members
        ViewBuilders.UpdateFrameworkElement (prevOpt, curr, target)
        // Update properties
        ViewUpdaters.updateStackPanelChildren prevStackPanelChildrenOpt currStackPanelChildrenOpt target
        match prevStackPanelOrientationOpt, currStackPanelOrientationOpt with
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
    member this.HorizontalAlignment = element.GetAttributeKeyed(ViewAttributes.FrameworkElementHorizontalAlignmentAttribKey)
    /// Get the value of the Margin member
    member this.Margin = element.GetAttributeKeyed(ViewAttributes.FrameworkElementMarginAttribKey)
    /// Get the value of the VerticalAlignment member
    member this.VerticalAlignment = element.GetAttributeKeyed(ViewAttributes.FrameworkElementVerticalAlignmentAttribKey)
    /// Get the value of the Width member
    member this.Width = element.GetAttributeKeyed(ViewAttributes.FrameworkElementWidthAttribKey)

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
    member this.Title = element.GetAttributeKeyed(ViewAttributes.WindowTitleAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a ButtonBase
type ButtonBaseViewer(element: ViewElement) =
    inherit ContentControlViewer(element)
    do if not ((typeof<System.Windows.Controls.Primitives.ButtonBase>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.Primitives.ButtonBase' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Command member
    member this.Command = element.GetAttributeKeyed(ViewAttributes.ButtonBaseCommandAttribKey)
    /// Get the value of the CommandCanExecute member
    member this.CommandCanExecute = element.GetAttributeKeyed(ViewAttributes.ButtonBaseCommandCanExecuteAttribKey)

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
    member this.IsChecked = element.GetAttributeKeyed(ViewAttributes.ToggleButtonIsCheckedAttribKey)
    /// Get the value of the Checked member
    member this.Checked = element.GetAttributeKeyed(ViewAttributes.ToggleButtonCheckedAttribKey)
    /// Get the value of the Unchecked member
    member this.Unchecked = element.GetAttributeKeyed(ViewAttributes.ToggleButtonUncheckedAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a CheckBox
type CheckBoxViewer(element: ViewElement) =
    inherit ToggleButtonViewer(element)
    do if not ((typeof<System.Windows.Controls.CheckBox>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.CheckBox' is expected, but '%s' was provided." element.TargetType.FullName

/// Viewer that allows to read the properties of a ViewElement representing a TextBlock
type TextBlockViewer(element: ViewElement) =
    inherit FrameworkElementViewer(element)
    do if not ((typeof<System.Windows.Controls.TextBlock>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.TextBlock' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Text member
    member this.Text = element.GetAttributeKeyed(ViewAttributes.TextBlockTextAttribKey)
    /// Get the value of the TextAlignment member
    member this.TextAlignment = element.GetAttributeKeyed(ViewAttributes.TextBlockTextAlignmentAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a RangeBase
type RangeBaseViewer(element: ViewElement) =
    inherit FrameworkElementViewer(element)
    do if not ((typeof<System.Windows.Controls.Primitives.RangeBase>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.Primitives.RangeBase' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Minimum member
    member this.Minimum = element.GetAttributeKeyed(ViewAttributes.RangeBaseMinimumAttribKey)
    /// Get the value of the Maximum member
    member this.Maximum = element.GetAttributeKeyed(ViewAttributes.RangeBaseMaximumAttribKey)
    /// Get the value of the Value member
    member this.Value = element.GetAttributeKeyed(ViewAttributes.RangeBaseValueAttribKey)
    /// Get the value of the ValueChanged member
    member this.ValueChanged = element.GetAttributeKeyed(ViewAttributes.RangeBaseValueChangedAttribKey)

/// Viewer that allows to read the properties of a ViewElement representing a Slider
type SliderViewer(element: ViewElement) =
    inherit RangeBaseViewer(element)
    do if not ((typeof<System.Windows.Controls.Slider>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.Slider' is expected, but '%s' was provided." element.TargetType.FullName

/// Viewer that allows to read the properties of a ViewElement representing a StackPanel
type StackPanelViewer(element: ViewElement) =
    inherit FrameworkElementViewer(element)
    do if not ((typeof<System.Windows.Controls.StackPanel>).IsAssignableFrom(element.TargetType)) then failwithf "A ViewElement assignable to type 'System.Windows.Controls.StackPanel' is expected, but '%s' was provided." element.TargetType.FullName
    /// Get the value of the Children member
    member this.Children = element.GetAttributeKeyed(ViewAttributes.StackPanelChildrenAttribKey)
    /// Get the value of the Orientation member
    member this.Orientation = element.GetAttributeKeyed(ViewAttributes.StackPanelOrientationAttribKey)

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

        /// Adjusts the FrameworkElementHorizontalAlignment property in the visual element
        member x.FrameworkElementHorizontalAlignment(value: System.Windows.HorizontalAlignment) = x.WithAttribute(ViewAttributes.FrameworkElementHorizontalAlignmentAttribKey, (value))

        /// Adjusts the FrameworkElementMargin property in the visual element
        member x.FrameworkElementMargin(value: System.Windows.Thickness) = x.WithAttribute(ViewAttributes.FrameworkElementMarginAttribKey, (value))

        /// Adjusts the FrameworkElementVerticalAlignment property in the visual element
        member x.FrameworkElementVerticalAlignment(value: System.Windows.VerticalAlignment) = x.WithAttribute(ViewAttributes.FrameworkElementVerticalAlignmentAttribKey, (value))

        /// Adjusts the FrameworkElementWidth property in the visual element
        member x.FrameworkElementWidth(value: float) = x.WithAttribute(ViewAttributes.FrameworkElementWidthAttribKey, (value))

        /// Adjusts the ContentControlContent property in the visual element
        member x.ContentControlContent(value: ViewElement) = x.WithAttribute(ViewAttributes.ContentControlContentAttribKey, (value))

        /// Adjusts the WindowTitle property in the visual element
        member x.WindowTitle(value: string) = x.WithAttribute(ViewAttributes.WindowTitleAttribKey, (value))

        /// Adjusts the ButtonBaseCommand property in the visual element
        member x.ButtonBaseCommand(value: unit -> unit) = x.WithAttribute(ViewAttributes.ButtonBaseCommandAttribKey, (value))

        /// Adjusts the ButtonBaseCommandCanExecute property in the visual element
        member x.ButtonBaseCommandCanExecute(value: bool) = x.WithAttribute(ViewAttributes.ButtonBaseCommandCanExecuteAttribKey, (value))

        /// Adjusts the ButtonContent property in the visual element
        member x.ButtonContent(value: string) = x.WithAttribute(ViewAttributes.ButtonContentAttribKey, (value))

        /// Adjusts the ToggleButtonChecked property in the visual element
        member x.ToggleButtonChecked(value: unit -> unit) = x.WithAttribute(ViewAttributes.ToggleButtonCheckedAttribKey, (fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))(value))

        /// Adjusts the ToggleButtonUnchecked property in the visual element
        member x.ToggleButtonUnchecked(value: unit -> unit) = x.WithAttribute(ViewAttributes.ToggleButtonUncheckedAttribKey, (fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))(value))

        /// Adjusts the ToggleButtonIsChecked property in the visual element
        member x.ToggleButtonIsChecked(value: bool option) = x.WithAttribute(ViewAttributes.ToggleButtonIsCheckedAttribKey, (value))

        /// Adjusts the TextBlockText property in the visual element
        member x.TextBlockText(value: string) = x.WithAttribute(ViewAttributes.TextBlockTextAttribKey, (value))

        /// Adjusts the TextBlockTextAlignment property in the visual element
        member x.TextBlockTextAlignment(value: System.Windows.TextAlignment) = x.WithAttribute(ViewAttributes.TextBlockTextAlignmentAttribKey, (value))

        /// Adjusts the RangeBaseValueChanged property in the visual element
        member x.RangeBaseValueChanged(value: float -> unit) = x.WithAttribute(ViewAttributes.RangeBaseValueChangedAttribKey, (fun f -> System.Windows.RoutedPropertyChangedEventHandler<float>(fun _sender _args -> f _args.NewValue))(value))

        /// Adjusts the RangeBaseMinimum property in the visual element
        member x.RangeBaseMinimum(value: float) = x.WithAttribute(ViewAttributes.RangeBaseMinimumAttribKey, (value))

        /// Adjusts the RangeBaseMaximum property in the visual element
        member x.RangeBaseMaximum(value: float) = x.WithAttribute(ViewAttributes.RangeBaseMaximumAttribKey, (value))

        /// Adjusts the RangeBaseValue property in the visual element
        member x.RangeBaseValue(value: float) = x.WithAttribute(ViewAttributes.RangeBaseValueAttribKey, (value))

        /// Adjusts the StackPanelChildren property in the visual element
        member x.StackPanelChildren(value: ViewElement list) = x.WithAttribute(ViewAttributes.StackPanelChildrenAttribKey, Array.ofList(value))

        /// Adjusts the StackPanelOrientation property in the visual element
        member x.StackPanelOrientation(value: System.Windows.Controls.Orientation) = x.WithAttribute(ViewAttributes.StackPanelOrientationAttribKey, (value))

        member inline x.With(?frameworkElementHorizontalAlignment: System.Windows.HorizontalAlignment, ?frameworkElementMargin: System.Windows.Thickness, ?frameworkElementVerticalAlignment: System.Windows.VerticalAlignment, ?frameworkElementWidth: float, ?contentControlContent: ViewElement, 
                             ?windowTitle: string, ?buttonBaseCommand: unit -> unit, ?buttonBaseCommandCanExecute: bool, ?buttonContent: string, ?toggleButtonChecked: unit -> unit, 
                             ?toggleButtonUnchecked: unit -> unit, ?toggleButtonIsChecked: bool option, ?textBlockText: string, ?textBlockTextAlignment: System.Windows.TextAlignment, ?rangeBaseValueChanged: float -> unit, 
                             ?rangeBaseMinimum: float, ?rangeBaseMaximum: float, ?rangeBaseValue: float, ?stackPanelChildren: ViewElement list, ?stackPanelOrientation: System.Windows.Controls.Orientation) =
            let x = match frameworkElementHorizontalAlignment with None -> x | Some opt -> x.FrameworkElementHorizontalAlignment(opt)
            let x = match frameworkElementMargin with None -> x | Some opt -> x.FrameworkElementMargin(opt)
            let x = match frameworkElementVerticalAlignment with None -> x | Some opt -> x.FrameworkElementVerticalAlignment(opt)
            let x = match frameworkElementWidth with None -> x | Some opt -> x.FrameworkElementWidth(opt)
            let x = match contentControlContent with None -> x | Some opt -> x.ContentControlContent(opt)
            let x = match windowTitle with None -> x | Some opt -> x.WindowTitle(opt)
            let x = match buttonBaseCommand with None -> x | Some opt -> x.ButtonBaseCommand(opt)
            let x = match buttonBaseCommandCanExecute with None -> x | Some opt -> x.ButtonBaseCommandCanExecute(opt)
            let x = match buttonContent with None -> x | Some opt -> x.ButtonContent(opt)
            let x = match toggleButtonChecked with None -> x | Some opt -> x.ToggleButtonChecked(opt)
            let x = match toggleButtonUnchecked with None -> x | Some opt -> x.ToggleButtonUnchecked(opt)
            let x = match toggleButtonIsChecked with None -> x | Some opt -> x.ToggleButtonIsChecked(opt)
            let x = match textBlockText with None -> x | Some opt -> x.TextBlockText(opt)
            let x = match textBlockTextAlignment with None -> x | Some opt -> x.TextBlockTextAlignment(opt)
            let x = match rangeBaseValueChanged with None -> x | Some opt -> x.RangeBaseValueChanged(opt)
            let x = match rangeBaseMinimum with None -> x | Some opt -> x.RangeBaseMinimum(opt)
            let x = match rangeBaseMaximum with None -> x | Some opt -> x.RangeBaseMaximum(opt)
            let x = match rangeBaseValue with None -> x | Some opt -> x.RangeBaseValue(opt)
            let x = match stackPanelChildren with None -> x | Some opt -> x.StackPanelChildren(opt)
            let x = match stackPanelOrientation with None -> x | Some opt -> x.StackPanelOrientation(opt)
            x

    /// Adjusts the FrameworkElementHorizontalAlignment property in the visual element
    let frameworkElementHorizontalAlignment (value: System.Windows.HorizontalAlignment) (x: ViewElement) = x.FrameworkElementHorizontalAlignment(value)
    /// Adjusts the FrameworkElementMargin property in the visual element
    let frameworkElementMargin (value: System.Windows.Thickness) (x: ViewElement) = x.FrameworkElementMargin(value)
    /// Adjusts the FrameworkElementVerticalAlignment property in the visual element
    let frameworkElementVerticalAlignment (value: System.Windows.VerticalAlignment) (x: ViewElement) = x.FrameworkElementVerticalAlignment(value)
    /// Adjusts the FrameworkElementWidth property in the visual element
    let frameworkElementWidth (value: float) (x: ViewElement) = x.FrameworkElementWidth(value)
    /// Adjusts the ContentControlContent property in the visual element
    let contentControlContent (value: ViewElement) (x: ViewElement) = x.ContentControlContent(value)
    /// Adjusts the WindowTitle property in the visual element
    let windowTitle (value: string) (x: ViewElement) = x.WindowTitle(value)
    /// Adjusts the ButtonBaseCommand property in the visual element
    let buttonBaseCommand (value: unit -> unit) (x: ViewElement) = x.ButtonBaseCommand(value)
    /// Adjusts the ButtonBaseCommandCanExecute property in the visual element
    let buttonBaseCommandCanExecute (value: bool) (x: ViewElement) = x.ButtonBaseCommandCanExecute(value)
    /// Adjusts the ButtonContent property in the visual element
    let buttonContent (value: string) (x: ViewElement) = x.ButtonContent(value)
    /// Adjusts the ToggleButtonChecked property in the visual element
    let toggleButtonChecked (value: unit -> unit) (x: ViewElement) = x.ToggleButtonChecked(value)
    /// Adjusts the ToggleButtonUnchecked property in the visual element
    let toggleButtonUnchecked (value: unit -> unit) (x: ViewElement) = x.ToggleButtonUnchecked(value)
    /// Adjusts the ToggleButtonIsChecked property in the visual element
    let toggleButtonIsChecked (value: bool option) (x: ViewElement) = x.ToggleButtonIsChecked(value)
    /// Adjusts the TextBlockText property in the visual element
    let textBlockText (value: string) (x: ViewElement) = x.TextBlockText(value)
    /// Adjusts the TextBlockTextAlignment property in the visual element
    let textBlockTextAlignment (value: System.Windows.TextAlignment) (x: ViewElement) = x.TextBlockTextAlignment(value)
    /// Adjusts the RangeBaseValueChanged property in the visual element
    let rangeBaseValueChanged (value: float -> unit) (x: ViewElement) = x.RangeBaseValueChanged(value)
    /// Adjusts the RangeBaseMinimum property in the visual element
    let rangeBaseMinimum (value: float) (x: ViewElement) = x.RangeBaseMinimum(value)
    /// Adjusts the RangeBaseMaximum property in the visual element
    let rangeBaseMaximum (value: float) (x: ViewElement) = x.RangeBaseMaximum(value)
    /// Adjusts the RangeBaseValue property in the visual element
    let rangeBaseValue (value: float) (x: ViewElement) = x.RangeBaseValue(value)
    /// Adjusts the StackPanelChildren property in the visual element
    let stackPanelChildren (value: ViewElement list) (x: ViewElement) = x.StackPanelChildren(value)
    /// Adjusts the StackPanelOrientation property in the visual element
    let stackPanelOrientation (value: System.Windows.Controls.Orientation) (x: ViewElement) = x.StackPanelOrientation(value)

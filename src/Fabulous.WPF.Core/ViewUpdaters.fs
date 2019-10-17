namespace Fabulous.WPF

open System.Collections
open Fabulous
open System.Windows.Input

module ViewUpdaters =
    /// Update a control given the previous and new view elements
    let inline updateChild (prevChild: ViewElement) (newChild:ViewElement) targetChild = 
        newChild.UpdateIncremental(prevChild, targetChild)

    /// Incremental list maintenance: given a collection, and a previous version of that collection, perform
    /// a reduced number of clear/add/remove/insert operations
    let updateCollectionGeneric
           (prevCollOpt: 'T[] voption) 
           (collOpt: 'T[] voption) 
           (targetColl: IList) 
           (create: 'T -> 'TargetT)
           (attach: 'T voption -> 'T -> 'TargetT -> unit) // adjust attached properties
           (canReuse : 'T -> 'T -> bool) // Used to check if reuse is possible
           (update: 'T -> 'T -> 'TargetT -> unit) // Incremental element-wise update, only if element reuse is allowed
        =
        match prevCollOpt, collOpt with 
        | ValueSome prevColl, ValueSome newColl when identical prevColl newColl -> ()
        | _, ValueNone -> targetColl.Clear()
        | _, ValueSome coll ->
            if (coll = null || coll.Length = 0) then
                targetColl.Clear()
            else
                // Remove the excess targetColl
                while (targetColl.Count > coll.Length) do
                    targetColl.RemoveAt (targetColl.Count - 1)

                // Count the existing targetColl
                // Unused variable n' introduced as a temporary workaround for https://github.com/fsprojects/Fabulous/issues/343
                let _ = targetColl.Count
                let n = targetColl.Count

                // Adjust the existing targetColl and create the new targetColl
                for i in 0 .. coll.Length-1 do
                    let newChild = coll.[i]
                    let prevChildOpt = match prevCollOpt with ValueNone -> ValueNone | ValueSome coll when i < n -> ValueSome coll.[i] | _ -> ValueNone
                    let prevChildOpt, targetChild = 
                        if (match prevChildOpt with ValueNone -> true | ValueSome prevChild -> not (identical prevChild newChild)) then
                            let mustCreate = (i >= n || match prevChildOpt with ValueNone -> true | ValueSome prevChild -> not (canReuse prevChild newChild))
                            if mustCreate then
                                let targetChild = create newChild
                                if i >= n then
                                    targetColl.Insert(i, targetChild)
                                else
                                    targetColl.[i] <- targetChild
                                ValueNone, targetChild
                            else
                                let targetChild = targetColl.[i]
                                update prevChildOpt.Value newChild targetChild
                                prevChildOpt, targetChild
                        else
                            prevChildOpt, targetColl.[i]
                    attach prevChildOpt newChild targetChild

    let updateStackPanelChildren (prev: ViewElement array voption) (curr: ViewElement array voption) (target: System.Windows.Controls.StackPanel) =
        updateCollectionGeneric prev curr target.Children
            (fun (x:ViewElement) -> x.Create())
            (fun _ _ _ -> ())
            ViewHelpers.canReuseView
            updateChild

    /// Converts an F# function to a WPF ICommand
    let makeCommand f =
        let ev = Event<_,_>()
        { new ICommand with
            member __.add_CanExecuteChanged h = ev.Publish.AddHandler h
            member __.remove_CanExecuteChanged h = ev.Publish.RemoveHandler h
            member __.CanExecute _ = true
            member __.Execute _ = f() }

    /// Converts an F# function to a WPF ICommand, with a CanExecute value
    let makeCommandCanExecute f canExecute =
        let ev = Event<_,_>()
        { new ICommand with
            member __.add_CanExecuteChanged h = ev.Publish.AddHandler h
            member __.remove_CanExecuteChanged h = ev.Publish.RemoveHandler h
            member __.CanExecute _ = canExecute
            member __.Execute _ = f() }

    /// Update the Command and CanExecute properties of a control, given previous and current values
    let inline updateCommand prevCommandValueOpt commandValueOpt argTransform setter  prevCanExecuteValueOpt canExecuteValueOpt target = 
        match prevCommandValueOpt, prevCanExecuteValueOpt, commandValueOpt, canExecuteValueOpt with 
        | ValueNone, ValueNone, ValueNone, ValueNone -> ()
        | ValueSome prevf, ValueNone, ValueSome f, ValueNone when identical prevf f -> ()
        | ValueSome prevf, ValueSome prevx, ValueSome f, ValueSome x when identical prevf f && prevx = x -> ()
        | _, _, ValueNone, _ -> setter target null
        | _, _, ValueSome f, ValueNone -> setter target (makeCommand (fun () -> f (argTransform target)))
        | _, _, ValueSome f, ValueSome k -> setter target (makeCommandCanExecute (fun () -> f (argTransform target)) k)
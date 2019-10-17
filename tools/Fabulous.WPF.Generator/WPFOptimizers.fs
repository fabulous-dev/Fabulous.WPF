namespace Fabulous.WPF.Generator

open Fabulous.CodeGen
open Fabulous.CodeGen.Binder
open Fabulous.CodeGen.Binder.Models

module WPFOptimizers =    
    /// Optimize command properties by asking for an F# function for the input type instead of ICommand
    module OptimizeCommands =
        let private canBeOptimized (boundProperty: BoundProperty) =
            boundProperty.ModelType = "System.Windows.Input.ICommand"
        
        let optimizeBoundProperty (boundType: BoundType) (boundProperty: BoundProperty) =
            [|
                // Accepts a function but don't apply it now
                { boundProperty with
                    InputType = "unit -> unit"
                    ModelType = "unit -> unit"
                    ConvertInputToModel = ""
                    ConvertModelToValue = ""
                    UpdateCode = "(fun _ _ _ -> ())" }
                
                // Accepts a boolean to know when the function can be executed
                // Creates a Command for both CanExecute and the function
                { Name = sprintf "%sCanExecute" boundProperty.Name
                  ShortName = sprintf "%sCanExecute" boundProperty.ShortName
                  UniqueName = sprintf "%sCanExecute" boundProperty.UniqueName
                  CanBeUpdated = true
                  DefaultValue = "true"
                  OriginalType = "bool"
                  InputType = "bool"
                  ModelType = "bool"
                  ConvertInputToModel = ""
                  ConvertModelToValue = ""
                  UpdateCode = sprintf "ViewUpdaters.updateCommand prev%sOpt curr%sOpt (fun _target -> ()) (fun (target: %s) cmd -> target.%s <- cmd)" boundProperty.UniqueName boundProperty.UniqueName boundType.Type boundProperty.Name
                  CollectionData = None
                  IsInherited = false }
            |]
        
        let apply = Optimizer.propertyOptimizer (fun _ prop -> canBeOptimized prop) optimizeBoundProperty
  
    
    /// Optimize events by storing them as EventHandlers
    /// TODO: This optimizer is currently invalid. Fabulous.CodeGen currently hides the real EventHandlerType
    module OptimizeRoutedEvents =
        let private canBeOptimized (boundEvent: BoundEvent) =
            true
    
        let private optimizeBoundEvent (boundEvent: BoundEvent) =
            { boundEvent with
                  ModelType =
                      match boundEvent.ModelType with
                      | "System.EventHandler" -> "System.Windows.RoutedEventHandler"
                      | _ -> boundEvent.ModelType
                  ConvertInputToModel =
                      match boundEvent.ModelType with
                      | "System.EventHandler" -> "(fun f -> System.Windows.RoutedEventHandler(fun _sender _args -> f()))"
                      | _ when not (System.String.IsNullOrWhiteSpace(boundEvent.EventArgsType)) -> sprintf "(fun f -> System.Windows.RoutedPropertyChangedEventHandler<%s>(fun _sender _args -> f _args.NewValue))" boundEvent.EventArgsType
                      | _ -> boundEvent.ConvertInputToModel }
        
        let apply = Optimizer.eventOptimizer (fun _ evt -> canBeOptimized evt) (fun _ evt -> [| optimizeBoundEvent evt |])
    
    /// Optimize nullables by asking for an F# option instead
    module OptimizeNullables =
        let private canBeOptimized (boundProperty: BoundProperty) =
            boundProperty.ModelType = "System.Nullable<System.Boolean>"
        
        let optimizeBoundProperty (boundProperty: BoundProperty) =
            { boundProperty with
                InputType = "bool option"
                ModelType = "bool option"
                ConvertModelToValue = "Option.toNullable" }
        
        let apply = Optimizer.propertyOptimizer (fun _ prop -> canBeOptimized prop) (fun _ prop -> [| optimizeBoundProperty prop |])
  
    let optimize =
        let wpfOptimize boundModel =
            boundModel
            |> OptimizeCommands.apply
            |> OptimizeRoutedEvents.apply
            |> OptimizeNullables.apply
        
        Optimizer.optimize
        >> WorkflowResult.map wpfOptimize


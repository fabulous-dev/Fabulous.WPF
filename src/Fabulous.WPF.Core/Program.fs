namespace Fabulous.WPF

open Fabulous
open System
open System.Windows

type WPFHost(app: Application) =
    interface IHost with
        member __.GetRootView() =
            match app.MainWindow with
            | null -> failwith "No root view"
            | rootView -> rootView :> obj 

        member __.SetRootView(rootView) =
            match rootView with
            | :? Window as window -> app.MainWindow <- window; window.Show()
            | _ -> failwithf "Incorrect model type: expected a window but got a %O" (rootView.GetType())

/// Program module - functions to manipulate program instances
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module WPFProgram =    
    let private syncDispatch (dispatch: 'msg -> unit) =
        fun msg ->
            Application.Current.Dispatcher.Invoke(Action(fun () -> dispatch msg))
            
    let private syncAction (fn: unit -> unit) =
        fun () ->
            Application.Current.Dispatcher.Invoke(Action(fun () -> fn()))

    let runWith app arg program =
        let host = WPFHost(app)

        program
        |> Program.withCanReuseView ViewHelpers.canReuseView
        |> Program.withSyncDispatch syncDispatch
        |> Program.withSyncAction syncAction
        |> Program.runWithFabulous host arg
        
    let run app program =
        runWith app () program
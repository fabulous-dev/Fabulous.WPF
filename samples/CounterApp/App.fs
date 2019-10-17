namespace CounterApp

open Fabulous
open Fabulous.WPF
open System.Windows
open System.Windows.Controls

module App =
    type Model = 
      { Count : int 
        Step : int
        TimerOn : bool }

    type Msg = 
        | Increment 
        | Decrement 
        | Reset
        | SetStep of int
        | TimerToggled of bool
        | TimedTick

    type CmdMsg =
        | TickTimer
        
    let timerCmd () =
        async { do! Async.Sleep 200
                return TimedTick }
        |> Cmd.ofAsyncMsg
        
    let mapCmdMsgToCmd cmdMsg =
        match cmdMsg with
        | TickTimer -> timerCmd()
        
    let initModel () = { Count = 0; Step = 1; TimerOn=false }
        
    let init () = initModel () , []
    
    let update msg model =
        match msg with
        | Increment -> { model with Count = model.Count + model.Step }, []
        | Decrement -> { model with Count = model.Count - model.Step }, []
        | Reset -> init ()
        | SetStep n -> { model with Step = n }, []
        | TimerToggled on -> { model with TimerOn = on }, (if on then [ TickTimer ] else [])
        | TimedTick -> if model.TimerOn then { model with Count = model.Count + model.Step }, [ TickTimer ] else model, [] 

    let view model dispatch =
        View.Window(
            title = "Fabulous.WPF CounterApp sample",
            content = View.StackPanel(
                verticalAlignment = VerticalAlignment.Center,
                margin = Thickness 30.,
                children = [
                    View.TextBlock(
                        text = sprintf "%d" model.Count,
                        horizontalAlignment = HorizontalAlignment.Center,
                        width = 200.,
                        textAlignment = TextAlignment.Center
                    )
                    View.Button(
                        content = "Increment",
                        command = fun() -> dispatch Increment
                    )
                    View.Button(
                        content = "Decrement",
                        command = fun() -> dispatch Decrement
                    )
                    View.StackPanel(
                        margin = Thickness 20.,
                        orientation = Orientation.Horizontal,
                        horizontalAlignment = HorizontalAlignment.Center,
                        children = [
                            View.TextBlock "Timer"
                            View.CheckBox(
                                isChecked = Some model.TimerOn,
                                checked = (fun () -> dispatch (TimerToggled true)),
                                unchecked = (fun () -> dispatch (TimerToggled false))
                            )
                        ]
                    )
                    View.Slider(
                        minimum = 0.,
                        maximum = 10.,
                        value = double model.Step,
                        valueChanged = (fun args -> dispatch (SetStep (int (args + 0.5))))
                    )
                    View.TextBlock(
                        text = sprintf "Step size: %d" model.Step,
                        horizontalAlignment = HorizontalAlignment.Center
                    )
                    View.Button(
                        content = "Reset",
                        horizontalAlignment = HorizontalAlignment.Center,
                        command = (fun () -> dispatch Reset),
                        commandCanExecute = (model <> initModel ())
                    )
                ]
            )
        )
        
    let program = 
        Program.mkProgramWithCmdMsg init update view mapCmdMsgToCmd

type App() as app =
    inherit Application()
    do app.ShutdownMode <- ShutdownMode.OnMainWindowClose

    let runner =
        App.program
        |> Program.withConsoleTrace
        |> WPFProgram.run app

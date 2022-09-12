namespace CounterApp

open System.Windows
open Fabulous
open Fabulous.WPF

open type Fabulous.WPF.View

module App =
    type Model =
        { Count: int
          Step: int
          TimerOn: bool }

    type Msg =
        | Increment
        | Decrement
        | Reset
        | SetStep of float
        | TimerToggled of bool
        | TimedTick

    let initModel = { Count = 0; Step = 1; TimerOn = false }

    let timerCmd () =
        async {
            do! Async.Sleep 200
            return TimedTick
        }
        |> Cmd.ofAsyncMsg

    let init () = initModel, Cmd.none

    let update msg model =
        match msg with
        | Increment ->
            { model with
                  Count = model.Count + model.Step },
            Cmd.none
        | Decrement ->
            { model with
                  Count = model.Count - model.Step },
            Cmd.none
        | Reset -> initModel, Cmd.none
        | SetStep n -> { model with Step = int(n + 0.5) }, Cmd.none
        | TimerToggled on -> { model with TimerOn = on }, (if on then timerCmd() else Cmd.none)
        | TimedTick ->
            if model.TimerOn then
                { model with
                      Count = model.Count + model.Step },
                timerCmd()
            else
                model, Cmd.none

    let view model =
        Window(
            "CounterApp",
            (VStack() {
                TextBlock($"%d{model.Count}")
                    .centerText()

                Button("Increment", Increment)

                Button("Decrement", Decrement)

                CheckBox("Timer", model.TimerOn, TimerToggled)
                    .margin(20.)
                    .centerHorizontal()

                Slider(0.0, 10.0, double model.Step, SetStep)

                TextBlock($"Step size: %d{model.Step}")
                    .centerText()

                Button("Reset", Reset)
            })
                .margin(30.)
                .centerVertical()
        )

    let program = Program.statefulWithCmd init update view

type App() as app =
    inherit Application(ShutdownMode = ShutdownMode.OnMainWindowClose)
    do Program.start app App.program
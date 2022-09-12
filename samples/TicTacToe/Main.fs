namespace TicTacToe

open System

module Main =
    [<EntryPoint; STAThread>]
    let main argv =
        App().Run()
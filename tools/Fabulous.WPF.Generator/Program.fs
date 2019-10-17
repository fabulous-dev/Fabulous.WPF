namespace Fabulous.WPF.Generator

open System.Diagnostics
open CommandLine
open Fabulous.CodeGen
    
module Program =
    type Options = {
        [<Option('m', "Mapping file", Required = true, HelpText = "Mapping file")>] MappingFile: string
        [<Option('o', "Output file", Required = true, HelpText = "Output file")>] OutputFile: string
        [<Option('d', "Debug", Required = false, HelpText = "Debug")>] Debug: bool
    }
    
    let tryReadOptions args =
        let options = CommandLine.Parser.Default.ParseArguments<Options>(args)
        match options with
        | :? Parsed<Options> as parsedOptions -> Some (parsedOptions.Value)
        | _ -> None
        
    let configuration =
        { baseTypeName = "System.Windows.DependencyObject"
          propertyBaseType = "System.Windows.DependencyProperty" }

    [<EntryPoint>]
    let main args =
        use consoleOutput = System.Console.OpenStandardOutput()
        use traceListener = new TextWriterTraceListener(consoleOutput)
        Trace.Listeners.Add(traceListener) |> ignore
        
        match tryReadOptions args with
        | None ->
            Trace.TraceError "Missing required arguments"
            1
        | Some options ->            
            let result =
                Program.mkProgram
                    Reflection.loadAllAssemblies
                    Reflection.tryGetProperty
                    configuration
                |> Program.withDebug options.Debug
                |> Program.withWorkflow (fun workflow ->
                    { workflow with
                        optimize = WPFOptimizers.optimize })
                |> Program.withReadAssembliesConfiguration (fun config ->
                    { config with
                        isTypeResolvable = Reflection.isTypeResolvable
                        tryGetStringRepresentationOfDefaultValue = WPFConverters.tryGetStringRepresentationOfDefaultValue }
                )
                |> Program.run options.MappingFile options.OutputFile
                
            // Exit code
            match result with
            | Error errors ->
                errors |> List.iter Trace.TraceError
                1
                
            | Ok (messages, warnings) ->
                warnings |> List.iter Trace.TraceWarning
                messages |> List.iter Trace.TraceInformation
                0
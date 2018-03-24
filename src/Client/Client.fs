module Client

open Elmish
open Elmish.React

open Client.Types
open Client.View
open Fable.Core.JsInterop
open Fable.Import.Browser

let init () = 
  let model = ofJson<Model> !!window?__INIT_STATE__
  model, Cmd.Empty

let update msg (model : Model) =
  let model' =
    match model,  msg with
    | Some x, Increment -> Some (x + 1)
    | Some x, Decrement -> Some (x - 1)
    | None, Init (Ok x) -> Some x
    | _ -> None
  model', Cmd.none
  
#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReactHydrate "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run

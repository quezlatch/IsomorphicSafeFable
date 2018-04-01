module Client.View


open Fable.Helpers.React
open Fable.Helpers.React.Props
open Client.Types

let safeComponents =
  let intersperse sep ls =
    List.foldBack (fun x -> function
      | [] -> [x]
      | xs -> x::sep::xs) ls []

  let components =
    [
      "Giraffe", "https://github.com/giraffe-fsharp/Giraffe"
      "Fable", "http://fable.io"
      "Elmish", "https://fable-elmish.github.io/"
    ]
    |> List.map (fun (desc,link) -> a [ Href link ] [ str desc ] )
    |> intersperse (str ", ")
    |> span [ ]

  p [ ]
    [ strong [] [ str "Isomorphic SAFE Fable" ]
      str " powered by: "
      components ]

let show = function
| Some x -> string x
| None -> "Loading..."

let view (model:Model) dispatch =
  div []
    [ h1 [] [ str "Isomorphic SAFE Fable" ]
      p  [] [ str "The initial page is rendered on server with counter and everything" ]
      p  [] [ str "Press buttons to manipulate counter:" ]
      button [ OnClick (fun _ -> dispatch Decrement) ] [ str "-" ]
      div [] [ str (show model) ]
      button [ OnClick (fun _ -> dispatch Increment) ] [ str "+" ]
      safeComponents ]
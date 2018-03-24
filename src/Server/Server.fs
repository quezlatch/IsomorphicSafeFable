module Server.Main

open System
open System.IO
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection

open Giraffe
open Giraffe.GiraffeViewEngine
open Giraffe.Serialization.Json

open Newtonsoft.Json

open Client.Types
open Client.View
let clientPath = Path.Combine("..","Client") |> Path.GetFullPath
let port = 8085us
let assetsBaseUrl = "http://localhost:8080"

let initState: Model = Some 142
let getInitCounter () : Task<Model> = task { return initState }

let htmlTemplate =
  let content = Fable.Helpers.ReactServer.renderToString (view initState ignore)
  html []
    [ head [] 
        [
          meta [attr "description" "generated server side"]
        ]
      body []
        [ div [_id "elmish-app"] [ rawText content ]
          script []
            [ rawText (sprintf """
            var __INIT_STATE__ = %s
            """ (toJson (toJson initState))) ] // call toJson twice to output json as js string in html
          script [ _src (assetsBaseUrl + "/public/bundle.js") ] []
        ]
    ]
let webApp : HttpHandler =
  choose [
    route "/" >=> htmlView htmlTemplate
    route "/api/init" >=>
      fun next ctx ->
        task {
          let! counter = getInitCounter()
          return! Successful.OK counter next ctx
        }
  ]

let configureApp  (app : IApplicationBuilder) =
  app.UseStaticFiles()
     .UseGiraffe webApp


let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore
    // Configure JsonSerializer to use Fable.JsonConverter
    let fableJsonSettings = JsonSerializerSettings()
    fableJsonSettings.Converters.Add(Fable.JsonConverter())

    services.AddSingleton<IJsonSerializer>(
        NewtonsoftJsonSerializer(fableJsonSettings)) |> ignore

[<EntryPoint>]
let main _ =
  WebHost
    .CreateDefaultBuilder()
    .UseWebRoot(clientPath)
    .UseContentRoot(clientPath)
    .Configure(Action<IApplicationBuilder> configureApp)
    .ConfigureServices(configureServices)
    .UseUrls("http://0.0.0.0:" + port.ToString() + "/")
    .Build()
    .Run()
  0
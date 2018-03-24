module Server.Main

open System
open System.IO
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection

open Giraffe
open Giraffe.Serialization.Json

open Newtonsoft.Json

open Shared

let clientPath = Path.Combine("..","Client") |> Path.GetFullPath
let port = 8085us
let assetsBaseUrl = "http://localhost:8080"

let getInitCounter () : Task<Counter> = task { return 42 }

let webApp : HttpHandler =
  choose [
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
let main argv =
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
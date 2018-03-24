on ma' macbook cos i'm down wit da yoof

swap from suave to giraffe
* mono .paket/paket.exe remove -g Server Suave
* mono .paket/paket.exe add -g Server -p src/Server/Server.fsproj Giraffe
* mono .paket/paket.exe add -g Server -p src/Server/Server.fsproj Microsoft.AspNetCore.StaticFiles
* mono .paket/paket.exe add -g Server -p src/Server/Server.fsproj Fable.JsonConverter
* mono .paket/paket.exe update
* fanangle Server.fs

Split out views and types into separate project. seems to work...

replacing index.html with server side view (but not rendered react just yet). have had to bodge
webpack to get it to proxy to giraffe for / route...

on ma' macbook cos i'm down wit da yoof

swap from suave to giraffe
> mono .paket/paket.exe remove -g Server Suave
> mono .paket/paket.exe add -g Server -p src/Server/Server.fsproj Giraffe
> mono .paket/paket.exe add -g Server -p src/Server/Server.fsproj Microsoft.AspNetCore.StaticFiles
> mono .paket/paket.exe add -g Server -p src/Server/Server.fsproj Fable.JsonConverter
> mono .paket/paket.exe update
> fanangle Server.fs

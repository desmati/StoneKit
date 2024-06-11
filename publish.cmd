cls

dotnet build StoneKit.sln --configuration Release

@echo off

REM Set the NuGet API key from environment variable
set ApiKey=%NUGET_API_KEY%

REM Check if the API key is empty
if not defined ApiKey (
    cls
    echo API key not set. Please set the NUGET_API_KEY environment variable and restart the terminal.
    exit /b 1
)

for /R ".\Assets\Packages\" %%A in ("StoneKit.Core.Structs.Maybe.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\" %%A in ("StoneKit.Core.Structs.DateRange.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\" %%A in ("StoneKit.Core.Common.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\" %%A in ("StoneKit.Core.Structs.TypePair.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\" %%A in ("StoneKit.Core.Reflection.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\" %%A in ("StoneKit.TransverseMapper.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\" %%A in ("StoneKit.Configuration.InIParser.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\" %%A in ("StoneKit.Core.Structs.Age.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\" %%A in ("StoneKit.Infrastructure.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee" %%A in ("StoneKit.Trustee.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee.Host" %%A in ("StoneKit.Trustee.Host.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee.Providers.AzureKeyVault" %%A in ("StoneKit.Trustee.Providers.AzureKeyVault.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee.Providers.FileSystem" %%A in ("StoneKit.Trustee.Providers.FileSystem.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee.Providers.Git" %%A in ("StoneKit.Trustee.Providers.Git.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee.Providers.HashicorpVault" %%A in ("StoneKit.Trustee.Providers.HashicorpVault.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee.Publishers.Nat" %%A in ("StoneKit.Trustee.Publishers.Nat.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee.Publishers.RabbitMq" %%A in ("StoneKit.Trustee.Publishers.RabbitMq.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

for /R ".\Assets\Packages\StoneKit.Trustee.Publishers.Redis" %%A in ("StoneKit.Trustee.Publishers.Redis.*.nupkg") do (set LatestPackage=%%A)
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

pause
cls

dotnet build StoneKit.sln --configuration Release

@echo off

REM Set the NuGet API key from environment variable
set ApiKey=%NUGET_API_KEY%

REM Check if the API key is empty
if not defined ApiKey (
    echo API key not set. Please set the NUGET_API_KEY environment variable.
    exit /b 1
)

for /R ".\Assets\Packages\" %%A in ("StoneKit.Core.Structs.Maybe.*.nupkg") do (set LatestPackage=%%A)
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

pause
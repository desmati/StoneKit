cls
@echo off

REM Set the NuGet API key from environment variable
set ApiKey=%NUGET_API_KEY%

REM Check if the API key is empty
if not defined ApiKey (
    echo API key not set. Please set the NUGET_API_KEY environment variable.
    exit /b 1
)

REM Get the latest .nupkg file in the directory
for /R ".\Assets\Packages\" %%A in ("StoneKit.Core.Structs.Maybe.*.nupkg") do (set LatestPackage=%%A)

REM Push the latest package to NuGet
"./Assets/nuget.exe" push %LatestPackage% -Source https://api.nuget.org/v3/index.json -ApiKey %ApiKey%

pause
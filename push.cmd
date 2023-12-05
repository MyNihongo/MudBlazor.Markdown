@echo off
setlocal

if "%1"=="" goto usage
if "%2"=="" goto good_to_go
goto usage
:good_to_go

set VERSION=%1

pushd src\MudBlazor.Markdown\bin\Release
nuget push MudBlazor.Markdown.%VERSION%.nupkg -src Librestream-Internal -ApiKey shawn
popd

exit /b 0

:usage
echo.
echo Usage^: %0 version
echo    version - a three part version number (major.minor.build)
echo.
goto :eof

@echo off
setlocal enabledelayedexpansion


:: ================================================
:: Variables
set version=0.1.8

set buildDir=%~dp0
set srcDir=%~dp0..\templates
set outputDir=%~dp0packed


:: ================================================
:: OutputDir
if not exist %outputDir% mkdir %outputDir%

:: Pack
for /r %srcDir% %%f in (*.nuspec) do (
  :: Variables
  set projectName=%%~nf
      
  :: Execute
  echo Packing project:%%~nf
  nuget pack "%%f" -OutputDirectory "%outputDir%" -Version "%version%"
)

:: Push
for /r %outputDir% %%f in (*.nupkg) do (
  echo Pushing package:%%~nf
  nuget push "%%f" -src https://api.nuget.org/v3/index.json
)

:: Clear
rmdir /s /q %outputDir%


:: ================================================
:: End
echo Completed.
exit /b
endlocal
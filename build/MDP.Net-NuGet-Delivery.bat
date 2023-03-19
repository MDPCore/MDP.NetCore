@echo off
setlocal enabledelayedexpansion


:: ================================================
:: Version
set version=6.0.16


:: ================================================
:: 03.Infrastructure - 01.Kernel
set project[30]=MDP.Data.SqlClient
set project[31]=MDP.Data.SqlClient.Hosting
set project[32]=MDP.IdentityModel.Tokens.Jwt
set project[33]=MDP.IdentityModel.Tokens.Jwt.Hosting

:: 03.Infrastructure - 02.Kernel
set project[9]=MDP.AspNetCore.Authentication
set project[10]=MDP.AspNetCore.Authentication.Hosting
set project[11]=MDP.AspNetCore.Authentication.AzureAD
set project[12]=MDP.AspNetCore.Authentication.AzureAD.Hosting
set project[13]=MDP.AspNetCore.Authentication.Facebook
set project[14]=MDP.AspNetCore.Authentication.Facebook.Hosting
set project[15]=MDP.AspNetCore.Authentication.GitHub
set project[16]=MDP.AspNetCore.Authentication.GitHub.Hosting
set project[17]=MDP.AspNetCore.Authentication.Google
set project[18]=MDP.AspNetCore.Authentication.Google.Hosting
set project[19]=MDP.AspNetCore.Authentication.Liff
set project[20]=MDP.AspNetCore.Authentication.Liff.Services
set project[21]=MDP.AspNetCore.Authentication.Liff.Hosting
set project[22]=MDP.AspNetCore.Authentication.Line
set project[23]=MDP.AspNetCore.Authentication.Line.Hosting
set project[24]=MDP.NetCore.Logging.Log4net
set project[25]=MDP.NetCore.Logging.Log4net.Hosting
set project[26]=MDP.NetCore.Logging.NLog
set project[27]=MDP.NetCore.Logging.NLog.Hosting
set project[28]=MDP.AspNetCore.Authentication.Jwt
set project[29]=MDP.AspNetCore.Authentication.Jwt.Hosting

:: 04.Kernel
set project[6]=MDP.AspNetCore
set project[7]=MDP.Hosting
set project[8]=MDP.NetCore

:: 05.Framework
set project[0]=CLK.Core
set project[1]=CLK.Diagnostics
set project[2]=CLK.IO
set project[3]=CLK.Mocks
set project[4]=CLK.Reflection
set project[5]=CLK.Security.Claims


:: ================================================
:: Pack
for /l %%n in (0,1,33) do ( 
   dotnet pack ../src/!project[%%n]!/!project[%%n]!.csproj -p:version=%version% -c release -o ./temp
)

:: Push
for /l %%n in (0,1,33) do ( 
   nuget push ./temp/!project[%%n]!.%version%.nupkg -src https://api.nuget.org/v3/index.json
)

:: Clear
rmdir /s /q temp
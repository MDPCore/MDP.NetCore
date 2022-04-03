#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SleepZone.Todos.WebApp/SleepZone.Todos.WebApp.csproj", "SleepZone.Todos.WebApp/"]
COPY ["SleepZone.Todos.WebPlatform/SleepZone.Todos.WebPlatform.csproj", "SleepZone.Todos.WebPlatform/"]
COPY ["MDP.Identity.Hosting/MDP.Identity.Hosting.csproj", "MDP.Identity.Hosting/"]
COPY ["MDP.Identity.Mocks/MDP.Identity.Mocks.csproj", "MDP.Identity.Mocks/"]
COPY ["CLK.Mocks/CLK.Mocks.csproj", "CLK.Mocks/"]
COPY ["CLK.Core/CLK.Core.csproj", "CLK.Core/"]
COPY ["MDP.Identity/MDP.Identity.csproj", "MDP.Identity/"]
COPY ["MDP.Hosting/MDP.Hosting.csproj", "MDP.Hosting/"]
COPY ["CLK.Reflection/CLK.Reflection.csproj", "CLK.Reflection/"]
COPY ["CLK.IO/CLK.IO.csproj", "CLK.IO/"]
COPY ["MDP.AspNetCore.Authentication/MDP.AspNetCore.Authentication.csproj", "MDP.AspNetCore.Authentication/"]
COPY ["SleepZone.Todos.Members.Hosting/SleepZone.Todos.Members.Hosting.csproj", "SleepZone.Todos.Members.Hosting/"]
COPY ["SleepZone.Todos.Members/SleepZone.Todos.Members.csproj", "SleepZone.Todos.Members/"]
COPY ["SleepZone.Todos.Members.Mocks/SleepZone.Todos.Members.Mocks.csproj", "SleepZone.Todos.Members.Mocks/"]
COPY ["SleepZone.Todos.Members.Accesses/SleepZone.Todos.Members.Accesses.csproj", "SleepZone.Todos.Members.Accesses/"]
COPY ["MDP.NetCore.Logging.NLog/MDP.NetCore.Logging.NLog.csproj", "MDP.NetCore.Logging.NLog/"]
COPY ["MDP.AspNetCore.Authentication.AzureAD/MDP.AspNetCore.Authentication.AzureAD.csproj", "MDP.AspNetCore.Authentication.AzureAD/"]
COPY ["MDP.AspNetCore.Authentication.GitHub/MDP.AspNetCore.Authentication.GitHub.csproj", "MDP.AspNetCore.Authentication.GitHub/"]
COPY ["MDP.AspNetCore.Authentication.Line.Services/MDP.AspNetCore.Authentication.Line.Services.csproj", "MDP.AspNetCore.Authentication.Line.Services/"]
COPY ["MDP.AspNetCore.Authentication.Line/MDP.AspNetCore.Authentication.Line.csproj", "MDP.AspNetCore.Authentication.Line/"]
COPY ["MDP.AspNetCore.Authentication.ExternalCookies/MDP.AspNetCore.Authentication.ExternalCookies.csproj", "MDP.AspNetCore.Authentication.ExternalCookies/"]
COPY ["MDP.NetCore.Logging.Log4net/MDP.NetCore.Logging.Log4net.csproj", "MDP.NetCore.Logging.Log4net/"]
COPY ["MDP.AspNetCore.Authentication.JwtBearer/MDP.AspNetCore.Authentication.JwtBearer.csproj", "MDP.AspNetCore.Authentication.JwtBearer/"]
COPY ["MDP.AspNetCore/MDP.AspNetCore.csproj", "MDP.AspNetCore/"]
COPY ["MDP.NetCore/MDP.NetCore.csproj", "MDP.NetCore/"]
COPY ["MDP.AspNetCore.Authentication.Policies/MDP.AspNetCore.Authentication.Policies.csproj", "MDP.AspNetCore.Authentication.Policies/"]
COPY ["SleepZone.Todos.Members.Identity.Hosting/SleepZone.Todos.Members.Identity.Hosting.csproj", "SleepZone.Todos.Members.Identity.Hosting/"]
COPY ["SleepZone.Todos.Members.Identity/SleepZone.Todos.Members.Identity.csproj", "SleepZone.Todos.Members.Identity/"]
COPY ["MDP.Module01.Hosting/MDP.Module01.Hosting.csproj", "MDP.Module01.Hosting/"]
COPY ["MDP.Module01/MDP.Module01.csproj", "MDP.Module01/"]
COPY ["MDP.Module01.Services/MDP.Module01.Services.csproj", "MDP.Module01.Services/"]
RUN dotnet restore "SleepZone.Todos.WebApp/SleepZone.Todos.WebApp.csproj"
COPY . .
WORKDIR "/src/SleepZone.Todos.WebApp"
RUN dotnet build "SleepZone.Todos.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SleepZone.Todos.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SleepZone.Todos.WebApp.dll"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SleepZone.Todos.WebApp/SleepZone.Todos.WebApp.csproj", "SleepZone.Todos.WebApp/"]
COPY ["MDP.WebPlatform/MDP.WebPlatform.csproj", "MDP.WebPlatform/"]
COPY ["MDP.NetCore.Logging.NLog/MDP.NetCore.Logging.NLog.csproj", "MDP.NetCore.Logging.NLog/"]
COPY ["MDP.AspNetCore.Authentication.ExternalCookies.Services/MDP.AspNetCore.Authentication.ExternalCookies.Services.csproj", "MDP.AspNetCore.Authentication.ExternalCookies.Services/"]
COPY ["MDP.AspNetCore.Authentication.ExternalCookies/MDP.AspNetCore.Authentication.ExternalCookies.csproj", "MDP.AspNetCore.Authentication.ExternalCookies/"]
COPY ["MDP.AspNetCore.Authentication.GitHub/MDP.AspNetCore.Authentication.GitHub.csproj", "MDP.AspNetCore.Authentication.GitHub/"]
COPY ["MDP.AspNetCore.Authentication.Line.Services/MDP.AspNetCore.Authentication.Line.Services.csproj", "MDP.AspNetCore.Authentication.Line.Services/"]
COPY ["MDP.AspNetCore.Authentication.Line/MDP.AspNetCore.Authentication.Line.csproj", "MDP.AspNetCore.Authentication.Line/"]
COPY ["MDP.NetCore.Logging.Log4net/MDP.NetCore.Logging.Log4net.csproj", "MDP.NetCore.Logging.Log4net/"]
COPY ["MDP.AspNetCore.Authentication.JwtBearer/MDP.AspNetCore.Authentication.JwtBearer.csproj", "MDP.AspNetCore.Authentication.JwtBearer/"]
COPY ["MDP.AspNetCore/MDP.AspNetCore.csproj", "MDP.AspNetCore/"]
COPY ["MDP.NetCore/MDP.NetCore.csproj", "MDP.NetCore/"]
COPY ["CLK.IO/CLK.IO.csproj", "CLK.IO/"]
COPY ["CLK.Reflection/CLK.Reflection.csproj", "CLK.Reflection/"]
COPY ["MDP.Hosting/MDP.Hosting.csproj", "MDP.Hosting/"]
COPY ["MDP.AspNetCore.Authentication.Policies/MDP.AspNetCore.Authentication.Policies.csproj", "MDP.AspNetCore.Authentication.Policies/"]
COPY ["SleepZone.Todos.Services/SleepZone.Todos.Services.csproj", "SleepZone.Todos.Services/"]
COPY ["SleepZone.Todos/SleepZone.Todos.csproj", "SleepZone.Todos/"]
COPY ["SleepZone.Todos.Hosting/SleepZone.Todos.Hosting.csproj", "SleepZone.Todos.Hosting/"]
COPY ["SleepZone.Todos.Mocks/SleepZone.Todos.Mocks.csproj", "SleepZone.Todos.Mocks/"]
COPY ["CLK.Mocks/CLK.Mocks.csproj", "CLK.Mocks/"]
COPY ["CLK.Core/CLK.Core.csproj", "CLK.Core/"]
COPY ["MDP.Data.SqlClient.Hosting/MDP.Data.SqlClient.Hosting.csproj", "MDP.Data.SqlClient.Hosting/"]
COPY ["CLK.Data.SqlClient/CLK.Data.SqlClient.csproj", "CLK.Data.SqlClient/"]
COPY ["SleepZone.Todos.Accesses/SleepZone.Todos.Accesses.csproj", "SleepZone.Todos.Accesses/"]
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
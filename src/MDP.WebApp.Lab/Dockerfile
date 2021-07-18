#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MDP.WebApp.Lab/MDP.WebApp.Lab.csproj", "MDP.WebApp.Lab/"]
COPY ["MDP.Module01.Hosting/MDP.Module01.Hosting.csproj", "MDP.Module01.Hosting/"]
COPY ["MDP.Module01/MDP.Module01.csproj", "MDP.Module01/"]
COPY ["MDP.Hosting/MDP.Hosting.csproj", "MDP.Hosting/"]
COPY ["CLK.Autofac/CLK.Autofac.csproj", "CLK.Autofac/"]
COPY ["MDP.Module01.Services/MDP.Module01.Services.csproj", "MDP.Module01.Services/"]
COPY ["MDP.AspNetCore/MDP.AspNetCore.csproj", "MDP.AspNetCore/"]
COPY ["MDP.NetCore/MDP.NetCore.csproj", "MDP.NetCore/"]
COPY ["CLK.IO/CLK.IO.csproj", "CLK.IO/"]
COPY ["CLK.Reflection/CLK.Reflection.csproj", "CLK.Reflection/"]
COPY ["MDP.WebApp/MDP.WebApp.csproj", "MDP.WebApp/"]
COPY ["MDP.NetCore.Logging.NLog/MDP.NetCore.Logging.NLog.csproj", "MDP.NetCore.Logging.NLog/"]
COPY ["MDP.AspNetCore.Authentication.ExternalCookies/MDP.AspNetCore.Authentication.ExternalCookies.csproj", "MDP.AspNetCore.Authentication.ExternalCookies/"]
COPY ["MDP.NetCore.Logging.Log4net/MDP.NetCore.Logging.Log4net.csproj", "MDP.NetCore.Logging.Log4net/"]
COPY ["MDP.AspNetCore.Authentication.JwtBearer/MDP.AspNetCore.Authentication.JwtBearer.csproj", "MDP.AspNetCore.Authentication.JwtBearer/"]
COPY ["MDP.AspNetCore.Authentication.Policies/MDP.AspNetCore.Authentication.Policies.csproj", "MDP.AspNetCore.Authentication.Policies/"]
RUN dotnet restore "MDP.WebApp.Lab/MDP.WebApp.Lab.csproj"
COPY . .
WORKDIR "/src/MDP.WebApp.Lab"
RUN dotnet build "MDP.WebApp.Lab.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MDP.WebApp.Lab.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MDP.WebApp.Lab.dll"]
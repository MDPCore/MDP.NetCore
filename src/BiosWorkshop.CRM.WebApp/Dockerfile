#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BiosWorkshop.CRM.WebApp/BiosWorkshop.CRM.WebApp.csproj", "BiosWorkshop.CRM.WebApp/"]
COPY ["BiosWorkshop.CRM.WebPlatform/BiosWorkshop.CRM.WebPlatform.csproj", "BiosWorkshop.CRM.WebPlatform/"]
COPY ["MDP.NetCore.Logging.NLog/MDP.NetCore.Logging.NLog.csproj", "MDP.NetCore.Logging.NLog/"]
COPY ["MDP.AspNetCore.Authentication.ExternalCookies.Services/MDP.AspNetCore.Authentication.ExternalCookies.Services.csproj", "MDP.AspNetCore.Authentication.ExternalCookies.Services/"]
COPY ["MDP.AspNetCore.Authentication.ExternalCookies/MDP.AspNetCore.Authentication.ExternalCookies.csproj", "MDP.AspNetCore.Authentication.ExternalCookies/"]
COPY ["MDP.AspNetCore.Authentication.Line.Services/MDP.AspNetCore.Authentication.Line.Services.csproj", "MDP.AspNetCore.Authentication.Line.Services/"]
COPY ["MDP.AspNetCore.Authentication.Line/MDP.AspNetCore.Authentication.Line.csproj", "MDP.AspNetCore.Authentication.Line/"]
COPY ["MDP.AspNetCore.Authentication.JwtBearer/MDP.AspNetCore.Authentication.JwtBearer.csproj", "MDP.AspNetCore.Authentication.JwtBearer/"]
COPY ["MDP.AspNetCore/MDP.AspNetCore.csproj", "MDP.AspNetCore/"]
COPY ["MDP.NetCore/MDP.NetCore.csproj", "MDP.NetCore/"]
COPY ["CLK.IO/CLK.IO.csproj", "CLK.IO/"]
COPY ["CLK.Reflection/CLK.Reflection.csproj", "CLK.Reflection/"]
COPY ["MDP.Hosting/MDP.Hosting.csproj", "MDP.Hosting/"]
COPY ["CLK.Autofac/CLK.Autofac.csproj", "CLK.Autofac/"]
COPY ["MDP.AspNetCore.Authentication.Policies/MDP.AspNetCore.Authentication.Policies.csproj", "MDP.AspNetCore.Authentication.Policies/"]
RUN dotnet restore "BiosWorkshop.CRM.WebApp/BiosWorkshop.CRM.WebApp.csproj"
COPY . .
WORKDIR "/src/BiosWorkshop.CRM.WebApp"
RUN dotnet build "BiosWorkshop.CRM.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BiosWorkshop.CRM.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BiosWorkshop.CRM.WebApp.dll"]
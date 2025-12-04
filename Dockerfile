#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LumoDevice/LumoDevice.API.csproj", "LumoDevice/"]
COPY ["API.Infrastructure/API.Infrastructure/API.Infrastructure.csproj", "API.Infrastructure/API.Infrastructure/"]
COPY ["DAL/DAL/DAL.csproj", "DAL/DAL/"]
RUN dotnet restore "./LumoDevice/./LumoDevice.API.csproj"
COPY . .
WORKDIR "/src/LumoDevice"
RUN dotnet build "./LumoDevice.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LumoDevice.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LumoDevice.API.dll"]
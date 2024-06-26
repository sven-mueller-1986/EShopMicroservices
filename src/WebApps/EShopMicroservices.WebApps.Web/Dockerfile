#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApps/EShopMicroservices.WebApps.Web/EShopMicroservices.WebApps.Web.csproj", "WebApps/EShopMicroservices.WebApps.Web/"]
COPY ["BuildingBlocks/EShopMicroservices.BuildingBlocks/EShopMicroservices.BuildingBlocks.csproj", "BuildingBlocks/EShopMicroservices.BuildingBlocks/"]
RUN dotnet restore "./WebApps/EShopMicroservices.WebApps.Web/EShopMicroservices.WebApps.Web.csproj"
COPY . .
WORKDIR "/src/WebApps/EShopMicroservices.WebApps.Web"
RUN dotnet build "./EShopMicroservices.WebApps.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShopMicroservices.WebApps.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopMicroservices.WebApps.Web.dll"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Basket/EShopMicroservices.Services.Basket.API/EShopMicroservices.Services.Basket.API.csproj", "Services/Basket/EShopMicroservices.Services.Basket.API/"]
COPY ["BuildingBlocks/EShopMicroservices.BuildingBlocks/EShopMicroservices.BuildingBlocks.csproj", "BuildingBlocks/EShopMicroservices.BuildingBlocks/"]
RUN dotnet restore "./Services/Basket/EShopMicroservices.Services.Basket.API/EShopMicroservices.Services.Basket.API.csproj"
COPY . .
WORKDIR "/src/Services/Basket/EShopMicroservices.Services.Basket.API"
RUN dotnet build "./EShopMicroservices.Services.Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShopMicroservices.Services.Basket.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopMicroservices.Services.Basket.API.dll"]
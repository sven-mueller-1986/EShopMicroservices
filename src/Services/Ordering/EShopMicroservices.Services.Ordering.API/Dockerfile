#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Ordering/EShopMicroservices.Services.Ordering.API/EShopMicroservices.Services.Ordering.API.csproj", "Services/Ordering/EShopMicroservices.Services.Ordering.API/"]
COPY ["Services/Ordering/EShopMicroservices.Services.Ordering.Application/EShopMicroservices.Services.Ordering.Application.csproj", "Services/Ordering/EShopMicroservices.Services.Ordering.Application/"]
COPY ["BuildingBlocks/EShopMicroservices.BuildingBlocks.Messaging/EShopMicroservices.BuildingBlocks.Messaging.csproj", "BuildingBlocks/EShopMicroservices.BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/EShopMicroservices.BuildingBlocks/EShopMicroservices.BuildingBlocks.csproj", "BuildingBlocks/EShopMicroservices.BuildingBlocks/"]
COPY ["Services/Ordering/EShopMicroservices.Services.Ordering.Domain/EShopMicroservices.Services.Ordering.Domain.csproj", "Services/Ordering/EShopMicroservices.Services.Ordering.Domain/"]
COPY ["Services/Ordering/EShopMicroservices.Services.Ordering.Infrastructure/EShopMicroservices.Services.Ordering.Infrastructure.csproj", "Services/Ordering/EShopMicroservices.Services.Ordering.Infrastructure/"]
RUN dotnet restore "./Services/Ordering/EShopMicroservices.Services.Ordering.API/EShopMicroservices.Services.Ordering.API.csproj"
COPY . .
WORKDIR "/src/Services/Ordering/EShopMicroservices.Services.Ordering.API"
RUN dotnet build "./EShopMicroservices.Services.Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShopMicroservices.Services.Ordering.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EShopMicroservices.Services.Ordering.API.dll"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Kcd.API/Kcd.Api.csproj", "Kcd.API/"]
COPY ["Kcd.Application/Kcd.Application.csproj", "Kcd.Application/"]
COPY ["Kcd.Domain/Kcd.Domain.csproj", "Kcd.Domain/"]
COPY ["Kcd.Common/Kcd.Common.csproj", "Kcd.Common/"]
COPY ["Kcd.Identity/Kcd.Identity.csproj", "Kcd.Identity/"]
COPY ["Kcd.Infrastructure/Kcd.Infrastructure.csproj", "Kcd.Infrastructure/"]
COPY ["Kcd.Persistence/Kcd.Persistence.csproj", "Kcd.Persistence/"]
RUN dotnet restore "./Kcd.API/Kcd.Api.csproj"
COPY . .
WORKDIR "/src/Kcd.API"
RUN dotnet build "./Kcd.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kcd.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kcd.Api.dll"]
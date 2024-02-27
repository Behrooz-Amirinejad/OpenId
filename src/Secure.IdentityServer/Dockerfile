#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Secure.IdentityServer/Secure.IdentityServer.csproj", "src/Secure.IdentityServer/"]
COPY ["src/SecureMicroservice.Movies.Api/SecureMicroservice.Movies.Api.csproj", "src/SecureMicroservice.Movies.Api/"]
RUN dotnet restore "./src/Secure.IdentityServer/./Secure.IdentityServer.csproj"
COPY . .
WORKDIR "/src/src/Secure.IdentityServer"
RUN dotnet build "./Secure.IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Secure.IdentityServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Secure.IdentityServer.dll"]
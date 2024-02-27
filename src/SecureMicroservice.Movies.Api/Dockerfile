FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SecureMicroservice.Movies.Api/SecureMicroservice.Movies.Api.csproj", "src/SecureMicroservice.Movies.Api/"]
RUN dotnet restore "src/SecureMicroservice.Movies.Api/SecureMicroservice.Movies.Api.csproj"

COPY . .
WORKDIR "/src/src/SecureMicroservice.Movies.Api"
RUN dotnet publish "SecureMicroservice.Movies.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=identityserver /app/publish/certificate.pfx .
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "SecureMicroservice.Movies.Api.dll"]
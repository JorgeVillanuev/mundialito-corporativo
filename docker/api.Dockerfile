# Contexto de build: la raíz del repo (no docker/), porque acá abajo se necesita copiar backend/src/.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY backend/src/Mundialito.Domain/Mundialito.Domain.csproj backend/src/Mundialito.Domain/
COPY backend/src/Mundialito.Application/Mundialito.Application.csproj backend/src/Mundialito.Application/
COPY backend/src/Mundialito.Infrastructure/Mundialito.Infrastructure.csproj backend/src/Mundialito.Infrastructure/
COPY backend/src/Mundialito.Api/Mundialito.Api.csproj backend/src/Mundialito.Api/
RUN dotnet restore backend/src/Mundialito.Api/Mundialito.Api.csproj

COPY backend/src/ backend/src/
RUN dotnet publish backend/src/Mundialito.Api/Mundialito.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Mundialito.Api.dll"]

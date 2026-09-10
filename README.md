# Mundialito de Fútbol Corporativo

Sistema de gestión de un torneo de fútbol corporativo: administración de equipos y jugadores, programación de partidos, registro de resultados, tabla de posiciones y ranking de goleadores.

## Arquitectura

Backend en .NET 8 con Clean Architecture (Domain, Application, Infrastructure, Api) y CQRS: los comandos (escritura) usan Entity Framework Core con Unit of Work, y las consultas (lectura) usan Dapper con SQL optimizado y paginación real en base de datos. Los endpoints de creación son idempotentes vía el header `Idempotency-Key`. Observabilidad con Serilog (logs estructurados, correlación de requests vía `X-Correlation-Id`).

Frontend en Next.js (App Router) con Tailwind CSS, consumiendo la Api directamente desde el navegador.

Diagrama completo de arquitectura: [`docs/Diagrama.svg`](docs/Diagrama.svg).

## Cómo levantar el proyecto

### Docker (recomendado)

```bash
docker-compose up --build
```

Levanta 3 servicios (`db`, `api`, `frontend`), aplica las migraciones de EF Core y siembra datos de ejemplo automáticamente — sin pasos manuales.

- Api: http://localhost:5000/api/v1 (Swagger: http://localhost:5000/swagger)
- Frontend: http://localhost:3000

Para reiniciar desde cero (borra los datos): `docker-compose down -v && docker-compose up --build`.

### Local (sin Docker para la Api)

1. Base de datos:
   ```bash
   docker run -d --name mundialito-sql-dev -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Mundialito2026!" -e "MSSQL_PID=Express" -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
   ```
2. Migraciones:
   ```bash
   dotnet ef database update --project backend/src/Mundialito.Infrastructure --startup-project backend/src/Mundialito.Api
   ```
3. Api (`Mundialito.Api` como proyecto de inicio, F5 en Visual Studio, o):
   ```bash
   dotnet run --project backend/src/Mundialito.Api
   ```
4. Frontend:
   ```bash
   cd frontend
   npm install
   npm run dev
   ```

## Endpoints principales

| Recurso | Métodos |
|---|---|
| `/api/v1/equipos` | `POST`, `GET`, `GET /{id}`, `PUT /{id}`, `DELETE /{id}` |
| `/api/v1/equipos/{id}/jugadores` | `POST`, `GET` |
| `/api/v1/partidos` | `POST`, `GET`, `GET /{id}`, `PUT /{id}/resultado` |
| `/api/v1/posiciones` | `GET` |
| `/api/v1/goleadores` | `GET` |

Los listados (`GET`) soportan paginación, y la mayoría admite filtros y ordenamiento vía query string. Documentación interactiva completa disponible en Swagger.

## Cómo correr los tests

```bash
dotnet test backend/tests/Mundialito.UnitTests
```

## Cómo probar con Postman

Con la Api corriendo:

```bash
newman run docs/postman/Mundialito.postman_collection.json --environment docs/postman/Mundialito.postman_environment.json
```

También se puede importar `docs/postman/Mundialito.postman_collection.json` y `docs/postman/Mundialito.postman_environment.json` directamente en Postman.

## Stack tecnológico

- **Backend**: .NET 8, ASP.NET Core, Entity Framework Core, Dapper, FluentValidation, Serilog, MSTest
- **Frontend**: Next.js (App Router), TypeScript, Tailwind CSS
- **Base de datos**: SQL Server 2022
- **Infraestructura**: Docker / Docker Compose

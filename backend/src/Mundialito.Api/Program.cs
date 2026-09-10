using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mundialito.Api.Middlewares;
using Mundialito.Application.Comun;
using Mundialito.Application.Equipos.Comandos;
using Mundialito.Application.Equipos.Consultas;
using Mundialito.Application.Jugadores.Comandos;
using Mundialito.Application.Jugadores.Consultas;
using Mundialito.Application.Partidos.Comandos;
using Mundialito.Application.Partidos.Consultas;
using Mundialito.Infrastructure.DependencyInjection;
using Mundialito.Infrastructure.Persistencia;
using Mundialito.Infrastructure.Seed;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, configuracionLog) =>
{
    configuracionLog
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File(new CompactJsonFormatter(), "logs/mundialito-.json", rollingInterval: RollingInterval.Day);
});

const string PoliticaCorsFrontend = "PoliticaCorsFrontend";
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy(PoliticaCorsFrontend, politica =>
        politica.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers()
    .AddJsonOptions(opciones => opciones.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new OpenApiInfo { Title = "Mundialito de Fútbol Corporativo", Version = "v1" });
});

builder.Services.AddValidatorsFromAssemblyContaining<CrearEquipoCommandValidator>();

builder.Services.AgregarInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICommandHandler<CrearEquipoCommand, Result<Guid>>, CrearEquipoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<ActualizarEquipoCommand, Result>, ActualizarEquipoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<EliminarEquipoCommand, Result>, EliminarEquipoCommandHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerEquiposQuery, PagedResult<EquipoDto>>, ObtenerEquiposQueryHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerEquipoPorIdQuery, Result<EquipoDto>>, ObtenerEquipoPorIdQueryHandler>();

builder.Services.AddScoped<ICommandHandler<RegistrarJugadorCommand, Result<Guid>>, RegistrarJugadorCommandHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerJugadoresPorEquipoQuery, Result<PagedResult<JugadorDto>>>, ObtenerJugadoresPorEquipoQueryHandler>();

builder.Services.AddScoped<ICommandHandler<CrearPartidoCommand, Result<Guid>>, CrearPartidoCommandHandler>();
builder.Services.AddScoped<ICommandHandler<RegistrarResultadoPartidoCommand, Result>, RegistrarResultadoPartidoCommandHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerPartidosQuery, PagedResult<PartidoDto>>, ObtenerPartidosQueryHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerPartidoPorIdQuery, Result<PartidoDto>>, ObtenerPartidoPorIdQueryHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerPosicionesQuery, PagedResult<PosicionDto>>, ObtenerPosicionesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<ObtenerGoleadoresQuery, PagedResult<GoleadorDto>>, ObtenerGoleadoresQueryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MiddlewareCorrelationId>();
app.UseMiddleware<MiddlewareManejoErrores>();

app.UseRouting();

app.UseCors(PoliticaCorsFrontend);

app.UseMiddleware<MiddlewareIdempotencia>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MundialitoDbContext>();
    await dbContext.Database.MigrateAsync();
    await DataSeeder.SembrarSiVacioAsync(dbContext);
}

app.Run();

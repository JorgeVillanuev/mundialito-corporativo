using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mundialito.Application.Comun;
using Mundialito.Infrastructure.EventosDominio;
using Mundialito.Infrastructure.Idempotencia;
using Mundialito.Infrastructure.Persistencia;
using Mundialito.Infrastructure.Persistencia.Consultas;
using Mundialito.Infrastructure.Persistencia.Repositorios;

namespace Mundialito.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AgregarInfrastructure(this IServiceCollection servicios, IConfiguration configuracion)
    {
        servicios.AddDbContext<MundialitoDbContext>(opciones =>
            opciones.UseSqlServer(
                configuracion.GetConnectionString("Default"),
                sqlOpciones => sqlOpciones.EnableRetryOnFailure()));

        servicios.AddScoped<IEquipoRepository, EquipoRepository>();
        servicios.AddScoped<IJugadorRepository, JugadorRepository>();
        servicios.AddScoped<IPartidoRepository, PartidoRepository>();

        servicios.AddScoped<IUnitOfWork, UnitOfWork>();

        servicios.AddScoped<IConexionFactory, SqlConexionFactory>();
        servicios.AddScoped<IEquipoQueryRepository, EquipoQueryRepository>();
        servicios.AddScoped<IJugadorQueryRepository, JugadorQueryRepository>();
        servicios.AddScoped<IPartidoQueryRepository, PartidoQueryRepository>();
        servicios.AddScoped<IPosicionesQueryRepository, PosicionesQueryRepository>();
        servicios.AddScoped<IGoleadoresQueryRepository, GoleadoresQueryRepository>();

        servicios.AddScoped<IIdempotenciaStore, IdempotenciaStore>();
        servicios.AddScoped<ServicioIdempotencia>();

        servicios.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return servicios;
    }
}

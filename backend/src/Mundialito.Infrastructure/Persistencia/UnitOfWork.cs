using Mundialito.Application.Comun;
using Mundialito.Domain.Comun;
using Mundialito.Infrastructure.EventosDominio;

namespace Mundialito.Infrastructure.Persistencia;

public class UnitOfWork : IUnitOfWork
{
    private readonly MundialitoDbContext _dbContext;

    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public UnitOfWork(
        MundialitoDbContext dbContext,
        IDomainEventDispatcher domainEventDispatcher,
        IEquipoRepository equipos,
        IJugadorRepository jugadores,
        IPartidoRepository partidos)
    {
        _dbContext = dbContext;
        _domainEventDispatcher = domainEventDispatcher;
        Equipos = equipos;
        Jugadores = jugadores;
        Partidos = partidos;
    }

    public IEquipoRepository Equipos { get; }
    public IJugadorRepository Jugadores { get; }
    public IPartidoRepository Partidos { get; }

    public async Task<int> GuardarCambiosAsync(CancellationToken cancellationToken)
    {
        var entidadesConEventos = _dbContext.ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.EventosDominio.Count > 0)
            .ToList();

        var resultado = await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var entidad in entidadesConEventos)
        {
            foreach (var evento in entidad.EventosDominio)
            {
                await _domainEventDispatcher.DespacharAsync(evento, cancellationToken);
            }

            entidad.LimpiarEventos();
        }

        return resultado;
    }
}

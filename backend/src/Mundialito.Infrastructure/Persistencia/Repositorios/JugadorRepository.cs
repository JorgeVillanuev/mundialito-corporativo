using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Comun;
using Mundialito.Domain.Entidades;

namespace Mundialito.Infrastructure.Persistencia.Repositorios;

public class JugadorRepository : IJugadorRepository
{
    private readonly MundialitoDbContext _dbContext;

    public JugadorRepository(MundialitoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Jugador>> ObtenerPorIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken)
    {
        return await _dbContext.Jugadores
            .Where(j => ids.Contains(j.Id))
            .ToListAsync(cancellationToken);
    }

    public void Agregar(Jugador jugador)
    {
        _dbContext.Jugadores.Add(jugador);
    }
}

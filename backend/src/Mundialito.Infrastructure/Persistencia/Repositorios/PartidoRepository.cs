using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Comun;
using Mundialito.Domain.Entidades;

namespace Mundialito.Infrastructure.Persistencia.Repositorios;

public class PartidoRepository : IPartidoRepository
{
    private readonly MundialitoDbContext _dbContext;

    public PartidoRepository(MundialitoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Partido?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Partidos.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public void Agregar(Partido partido)
    {
        _dbContext.Partidos.Add(partido);
    }

    public void AgregarGoles(IEnumerable<GolPartido> goles)
    {
        _dbContext.GolesPartido.AddRange(goles);
    }
}

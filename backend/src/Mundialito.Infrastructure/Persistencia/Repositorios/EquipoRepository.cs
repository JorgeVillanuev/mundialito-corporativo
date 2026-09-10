using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Comun;
using Mundialito.Domain.Entidades;

namespace Mundialito.Infrastructure.Persistencia.Repositorios;

public class EquipoRepository : IEquipoRepository
{
    private readonly MundialitoDbContext _dbContext;

    public EquipoRepository(MundialitoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Equipo?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.Equipos.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public Task<bool> ExisteConNombreAsync(string nombre, Guid? idAExcluir, CancellationToken cancellationToken)
    {
        return _dbContext.Equipos.AnyAsync(e => e.Nombre == nombre && (idAExcluir == null || e.Id != idAExcluir), cancellationToken);
    }

    public async Task<bool> TieneDependenciasAsync(Guid equipoId, CancellationToken cancellationToken)
    {
        var tieneJugadores = await _dbContext.Jugadores.AnyAsync(j => j.EquipoId == equipoId, cancellationToken);
        if (tieneJugadores)
            return true;

        return await _dbContext.Partidos.AnyAsync(p => p.EquipoLocalId == equipoId || p.EquipoVisitanteId == equipoId, cancellationToken);
    }

    public void Agregar(Equipo equipo)
    {
        _dbContext.Equipos.Add(equipo);
    }

    public void Eliminar(Equipo equipo)
    {
        _dbContext.Equipos.Remove(equipo);
    }
}

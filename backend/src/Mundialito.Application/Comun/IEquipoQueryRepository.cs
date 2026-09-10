using Mundialito.Application.Equipos.Consultas;

namespace Mundialito.Application.Comun;

public interface IEquipoQueryRepository
{
    Task<PagedResult<EquipoDto>> ObtenerAsync(ObtenerEquiposQuery consulta, CancellationToken cancellationToken);

    Task<EquipoDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken);
}

using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Application.Comun;

public interface IPartidoQueryRepository
{
    Task<PagedResult<PartidoDto>> ObtenerAsync(ObtenerPartidosQuery consulta, CancellationToken cancellationToken);

    Task<PartidoDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);
}

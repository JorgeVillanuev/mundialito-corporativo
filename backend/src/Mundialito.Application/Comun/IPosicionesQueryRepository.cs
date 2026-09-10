using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Application.Comun;

public interface IPosicionesQueryRepository
{
    Task<PagedResult<PosicionDto>> ObtenerAsync(ObtenerPosicionesQuery consulta, CancellationToken cancellationToken);
}

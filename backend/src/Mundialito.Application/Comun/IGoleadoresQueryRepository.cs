using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Application.Comun;

public interface IGoleadoresQueryRepository
{
    Task<PagedResult<GoleadorDto>> ObtenerAsync(ObtenerGoleadoresQuery consulta, CancellationToken cancellationToken);
}

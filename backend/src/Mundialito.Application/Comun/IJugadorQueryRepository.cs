using Mundialito.Application.Jugadores.Consultas;

namespace Mundialito.Application.Comun;

public interface IJugadorQueryRepository
{
    Task<PagedResult<JugadorDto>> ObtenerPorEquipoAsync(ObtenerJugadoresPorEquipoQuery consulta, CancellationToken cancellationToken);
}

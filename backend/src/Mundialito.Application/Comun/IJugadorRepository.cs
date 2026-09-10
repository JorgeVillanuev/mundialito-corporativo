using Mundialito.Domain.Entidades;

namespace Mundialito.Application.Comun;

public interface IJugadorRepository
{
    Task<IReadOnlyList<Jugador>> ObtenerPorIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);

    void Agregar(Jugador jugador);
}

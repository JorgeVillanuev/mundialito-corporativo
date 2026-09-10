using Mundialito.Domain.Entidades;

namespace Mundialito.Application.Comun;

public interface IPartidoRepository
{
    Task<Partido?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    void Agregar(Partido partido);

    void AgregarGoles(IEnumerable<GolPartido> goles);
}

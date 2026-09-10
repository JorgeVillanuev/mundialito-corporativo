using Mundialito.Domain.Comun;

namespace Mundialito.Domain.Entidades;

public class GolPartido : Entity
{
    public Guid PartidoId { get; private set; }

    public Guid JugadorId { get; private set; }

    public int CantidadGoles { get; private set; }

    private GolPartido() { }

    public static GolPartido Crear(Guid partidoId, Guid jugadorId, int cantidadGoles)
    {
        if (partidoId == Guid.Empty)
            throw new ArgumentException("El partido es obligatorio.", nameof(partidoId));
        if (jugadorId == Guid.Empty)
            throw new ArgumentException("El jugador es obligatorio.", nameof(jugadorId));
        if (cantidadGoles < 1)
            throw new ArgumentOutOfRangeException(nameof(cantidadGoles), "La cantidad de goles debe ser al menos 1.");

        return new GolPartido
        {
            PartidoId = partidoId,
            JugadorId = jugadorId,
            CantidadGoles = cantidadGoles
        };
    }
}

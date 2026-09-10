namespace Mundialito.Application.Partidos.Consultas;

public sealed class PosicionDto
{
    public Guid EquipoId { get; init; }

    public string NombreEquipo { get; init; } = string.Empty;

    public int PartidosJugados { get; init; }

    public int Victorias { get; init; }

    public int Empates { get; init; }

    public int Derrotas { get; init; }

    public int GolesFavor { get; init; }

    public int GolesContra { get; init; }

    public int DiferenciaGol { get; init; }

    public int Puntos { get; init; }

    public int Posicion { get; init; }
}

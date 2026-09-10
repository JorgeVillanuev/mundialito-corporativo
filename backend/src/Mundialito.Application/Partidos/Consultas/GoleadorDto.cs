namespace Mundialito.Application.Partidos.Consultas;

public sealed class GoleadorDto
{
    public Guid JugadorId { get; init; }

    public string NombreJugador { get; init; } = string.Empty;

    public string NombreEquipo { get; init; } = string.Empty;

    public int CantidadGoles { get; init; }
}

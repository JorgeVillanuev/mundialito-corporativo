namespace Mundialito.Application.Partidos.Comandos;

public sealed record GolJugadorEntrada(
    Guid JugadorId,
    int CantidadGoles
);

public sealed record RegistrarResultadoPartidoCommand(
    Guid PartidoId,
    int GolesLocal,
    int GolesVisitante,
    IReadOnlyList<GolJugadorEntrada> Goleadores
);

namespace Mundialito.Application.Partidos.Comandos;

public sealed record CrearPartidoCommand(
    Guid EquipoLocalId,
    Guid EquipoVisitanteId,
    DateTime FechaHora
);

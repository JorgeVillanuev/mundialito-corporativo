using Mundialito.Domain.Enums;

namespace Mundialito.Application.Partidos.Consultas;

public sealed class PartidoDto
{
    public Guid Id { get; init; }

    public Guid EquipoLocalId { get; init; }

    public string NombreEquipoLocal { get; init; } = string.Empty;

    public Guid EquipoVisitanteId { get; init; }

    public string NombreEquipoVisitante { get; init; } = string.Empty;

    public DateTime FechaHora { get; init; }

    public EstadoPartido Estado { get; init; }

    public int? GolesLocal { get; init; }

    public int? GolesVisitante { get; init; }
}

using Mundialito.Domain.Enums;

namespace Mundialito.Application.Partidos.Consultas;

public sealed record ObtenerPartidosQuery(
    DateTime? FechaDesde = null,
    DateTime? FechaHasta = null,
    Guid? EquipoId = null,
    EstadoPartido? Estado = null,
    int NumeroPagina = 1,
    int TamanoPagina = 10,
    string? OrdenarPor = null,
    string? DireccionOrden = null);

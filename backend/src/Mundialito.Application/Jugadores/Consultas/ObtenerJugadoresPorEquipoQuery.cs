using Mundialito.Domain.Enums;

namespace Mundialito.Application.Jugadores.Consultas;

public sealed record ObtenerJugadoresPorEquipoQuery(
    Guid EquipoId,
    string? Nombre = null,
    PosicionJugador? Posicion = null,
    int NumeroPagina = 1,
    int TamanoPagina = 10,
    string? OrdenarPor = null,
    string? DireccionOrden = null);

namespace Mundialito.Application.Equipos.Consultas;

public sealed record ObtenerEquiposQuery(
    string? Nombre = null,
    string? CiudadOrigen = null,
    int NumeroPagina = 1,
    int TamanoPagina = 10,
    string? OrdenarPor = null,
    string? DireccionOrden = null
);

namespace Mundialito.Application.Partidos.Consultas;

public sealed record ObtenerGoleadoresQuery(
    Guid? EquipoId = null,
    int NumeroPagina = 1,
    int TamanoPagina = 10,
    string? OrdenarPor = null,
    string? DireccionOrden = null);

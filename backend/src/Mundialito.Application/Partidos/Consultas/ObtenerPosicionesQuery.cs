namespace Mundialito.Application.Partidos.Consultas;

public sealed record ObtenerPosicionesQuery(
    int NumeroPagina = 1,
    int TamanoPagina = 10);

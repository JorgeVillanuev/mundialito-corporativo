namespace Mundialito.Application.Partidos.Consultas;

public static class CalculadorTablaPosiciones
{
    public static IReadOnlyList<PosicionDto> Ordenar(IEnumerable<EstadisticasEquipo> estadisticas)
    {
        return estadisticas
            .Select(e => new
            {
                Estadisticas = e,
                DiferenciaGol = e.GolesFavor - e.GolesContra,
                Puntos = CalculadoraPuntos.Calcular(e.Victorias, e.Empates)
            })
            .OrderByDescending(x => x.Puntos)
            .ThenByDescending(x => x.DiferenciaGol)
            .ThenByDescending(x => x.Estadisticas.GolesFavor)
            .ThenBy(x => x.Estadisticas.NombreEquipo, StringComparer.Ordinal)
            .Select((x, indice) => new PosicionDto
            {
                EquipoId = x.Estadisticas.EquipoId,
                NombreEquipo = x.Estadisticas.NombreEquipo,
                PartidosJugados = x.Estadisticas.PartidosJugados,
                Victorias = x.Estadisticas.Victorias,
                Empates = x.Estadisticas.Empates,
                Derrotas = x.Estadisticas.Derrotas,
                GolesFavor = x.Estadisticas.GolesFavor,
                GolesContra = x.Estadisticas.GolesContra,
                DiferenciaGol = x.DiferenciaGol,
                Puntos = x.Puntos,
                Posicion = indice + 1
            })
            .ToList();
    }
}

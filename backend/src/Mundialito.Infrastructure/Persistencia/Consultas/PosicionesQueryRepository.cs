using Dapper;
using Mundialito.Application.Comun;
using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Infrastructure.Persistencia.Consultas;

public class PosicionesQueryRepository : IPosicionesQueryRepository
{
    private const string Sql = """
        WITH ResultadosPorEquipo AS (
            SELECT
                p.EquipoLocalId AS EquipoId,
                CASE WHEN p.GolesLocal > p.GolesVisitante THEN 1 ELSE 0 END AS Victoria,
                CASE WHEN p.GolesLocal = p.GolesVisitante THEN 1 ELSE 0 END AS Empate,
                CASE WHEN p.GolesLocal < p.GolesVisitante THEN 1 ELSE 0 END AS Derrota,
                p.GolesLocal AS GolesFavor,
                p.GolesVisitante AS GolesContra
            FROM Partidos p
            WHERE p.Estado = 1

            UNION ALL

            SELECT
                p.EquipoVisitanteId AS EquipoId,
                CASE WHEN p.GolesVisitante > p.GolesLocal THEN 1 ELSE 0 END AS Victoria,
                CASE WHEN p.GolesVisitante = p.GolesLocal THEN 1 ELSE 0 END AS Empate,
                CASE WHEN p.GolesVisitante < p.GolesLocal THEN 1 ELSE 0 END AS Derrota,
                p.GolesVisitante AS GolesFavor,
                p.GolesLocal AS GolesContra
            FROM Partidos p
            WHERE p.Estado = 1
        )
        SELECT
            e.Id AS EquipoId,
            e.Nombre AS NombreEquipo,
            COUNT(r.EquipoId) AS PartidosJugados,
            ISNULL(SUM(r.Victoria), 0) AS Victorias,
            ISNULL(SUM(r.Empate), 0) AS Empates,
            ISNULL(SUM(r.Derrota), 0) AS Derrotas,
            ISNULL(SUM(r.GolesFavor), 0) AS GolesFavor,
            ISNULL(SUM(r.GolesContra), 0) AS GolesContra
        FROM Equipos e
        LEFT JOIN ResultadosPorEquipo r ON r.EquipoId = e.Id
        GROUP BY e.Id, e.Nombre
        """;

    private readonly IConexionFactory _conexionFactory;

    public PosicionesQueryRepository(IConexionFactory conexionFactory)
    {
        _conexionFactory = conexionFactory;
    }

    public async Task<PagedResult<PosicionDto>> ObtenerAsync(ObtenerPosicionesQuery consulta, CancellationToken cancellationToken)
    {
        using var conexion = _conexionFactory.CrearConexion();
        var comando = new CommandDefinition(Sql, cancellationToken: cancellationToken);
        var estadisticas = await conexion.QueryAsync<EstadisticasEquipo>(comando);

        var tablaCompleta = CalculadorTablaPosiciones.Ordenar(estadisticas);

        var pagina = tablaCompleta
            .Skip((consulta.NumeroPagina - 1) * consulta.TamanoPagina)
            .Take(consulta.TamanoPagina)
            .ToList();

        return new PagedResult<PosicionDto>(pagina, consulta.NumeroPagina, consulta.TamanoPagina, tablaCompleta.Count);
    }
}

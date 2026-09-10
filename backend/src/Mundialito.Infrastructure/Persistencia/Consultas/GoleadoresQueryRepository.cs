using Mundialito.Application.Comun;
using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Infrastructure.Persistencia.Consultas;

public class GoleadoresQueryRepository : IGoleadoresQueryRepository
{
    private static readonly Dictionary<string, string> ColumnasOrdenables = new(StringComparer.OrdinalIgnoreCase)
    {
        ["cantidadGoles"] = "CantidadGoles",
        ["nombreJugador"] = "NombreJugador"
    };

    private readonly IConexionFactory _conexionFactory;

    public GoleadoresQueryRepository(IConexionFactory conexionFactory)
    {
        _conexionFactory = conexionFactory;
    }

    public async Task<PagedResult<GoleadorDto>> ObtenerAsync(ObtenerGoleadoresQuery consulta, CancellationToken cancellationToken)
    {
        const string sqlBase = """
            FROM GolesPartido g
            INNER JOIN Partidos p ON p.Id = g.PartidoId
            INNER JOIN Jugadores j ON j.Id = g.JugadorId
            INNER JOIN Equipos e ON e.Id = j.EquipoId
            WHERE p.Estado = 1
              AND (@EquipoId IS NULL OR j.EquipoId = @EquipoId)
            """;

        var sqlSelect = $"""
            SELECT j.Id AS JugadorId, j.Nombre AS NombreJugador, e.Nombre AS NombreEquipo,
                   SUM(g.CantidadGoles) AS CantidadGoles
            {sqlBase}
            GROUP BY j.Id, j.Nombre, e.Nombre
            """;

        var sqlCount = $"""
            SELECT COUNT(*) FROM (
                SELECT j.Id
                {sqlBase}
                GROUP BY j.Id, j.Nombre, e.Nombre
            ) AS Conteo
            """;

        var ordenarPor = OrdenSql.Resolver(
            ColumnasOrdenables, consulta.OrdenarPor, consulta.DireccionOrden, "CantidadGoles", descendentePorDefecto: true);

        using var conexion = _conexionFactory.CrearConexion();
        return await PaginadorDapper.ObtenerPaginaAsync<GoleadorDto>(
            conexion,
            sqlSelect,
            sqlCount,
            ordenarPor,
            new { consulta.EquipoId },
            consulta.NumeroPagina,
            consulta.TamanoPagina,
            cancellationToken);
    }
}

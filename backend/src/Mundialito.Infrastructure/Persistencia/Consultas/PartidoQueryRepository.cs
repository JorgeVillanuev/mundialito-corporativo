using Dapper;
using Mundialito.Application.Comun;
using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Infrastructure.Persistencia.Consultas;

public class PartidoQueryRepository : IPartidoQueryRepository
{
    private static readonly Dictionary<string, string> ColumnasOrdenables = new(StringComparer.OrdinalIgnoreCase)
    {
        ["fechaHora"] = "p.FechaHora",
        ["estado"] = "p.Estado"
    };

    private readonly IConexionFactory _conexionFactory;

    public PartidoQueryRepository(IConexionFactory conexionFactory)
    {
        _conexionFactory = conexionFactory;
    }

    public async Task<PagedResult<PartidoDto>> ObtenerAsync(ObtenerPartidosQuery consulta, CancellationToken cancellationToken)
    {
        const string sqlBase = """
            FROM Partidos p
            INNER JOIN Equipos el ON el.Id = p.EquipoLocalId
            INNER JOIN Equipos ev ON ev.Id = p.EquipoVisitanteId
            WHERE (@FechaDesde IS NULL OR p.FechaHora >= @FechaDesde)
              AND (@FechaHasta IS NULL OR p.FechaHora <= @FechaHasta)
              AND (@EquipoId IS NULL OR p.EquipoLocalId = @EquipoId OR p.EquipoVisitanteId = @EquipoId)
              AND (@Estado IS NULL OR p.Estado = @Estado)
            """;

        var sqlSelect = $"""
            SELECT p.Id, p.EquipoLocalId, el.Nombre AS NombreEquipoLocal,
                   p.EquipoVisitanteId, ev.Nombre AS NombreEquipoVisitante,
                   p.FechaHora, p.Estado, p.GolesLocal, p.GolesVisitante
            {sqlBase}
            """;

        var sqlCount = $"""
            SELECT COUNT(*)
            {sqlBase}
            """;

        var ordenarPor = OrdenSql.Resolver(ColumnasOrdenables, consulta.OrdenarPor, consulta.DireccionOrden, "p.FechaHora");

        using var conexion = _conexionFactory.CrearConexion();
        return await PaginadorDapper.ObtenerPaginaAsync<PartidoDto>(
            conexion,
            sqlSelect,
            sqlCount,
            ordenarPor,
            new
            {
                consulta.FechaDesde,
                consulta.FechaHasta,
                consulta.EquipoId,
                Estado = (int?)consulta.Estado
            },
            consulta.NumeroPagina,
            consulta.TamanoPagina,
            cancellationToken);
    }

    public async Task<PartidoDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT p.Id, p.EquipoLocalId, el.Nombre AS NombreEquipoLocal,
                   p.EquipoVisitanteId, ev.Nombre AS NombreEquipoVisitante,
                   p.FechaHora, p.Estado, p.GolesLocal, p.GolesVisitante
            FROM Partidos p
            INNER JOIN Equipos el ON el.Id = p.EquipoLocalId
            INNER JOIN Equipos ev ON ev.Id = p.EquipoVisitanteId
            WHERE p.Id = @Id
            """;

        using var conexion = _conexionFactory.CrearConexion();
        var comando = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        return await conexion.QuerySingleOrDefaultAsync<PartidoDto>(comando);
    }
}

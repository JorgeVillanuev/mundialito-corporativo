using Dapper;
using Mundialito.Application.Comun;
using Mundialito.Application.Equipos.Consultas;

namespace Mundialito.Infrastructure.Persistencia.Consultas;

public class EquipoQueryRepository : IEquipoQueryRepository
{
    private static readonly Dictionary<string, string> ColumnasOrdenables = new(StringComparer.OrdinalIgnoreCase)
    {
        ["nombre"] = "Nombre",
        ["ciudadOrigen"] = "CiudadOrigen"
    };

    private readonly IConexionFactory _conexionFactory;

    public EquipoQueryRepository(IConexionFactory conexionFactory)
    {
        _conexionFactory = conexionFactory;
    }

    public async Task<PagedResult<EquipoDto>> ObtenerAsync(ObtenerEquiposQuery consulta, CancellationToken cancellationToken)
    {
        const string sqlSelect = """
            SELECT Id, Nombre, CiudadOrigen
            FROM Equipos
            WHERE (@Nombre IS NULL OR Nombre LIKE '%' + @Nombre + '%')
              AND (@CiudadOrigen IS NULL OR CiudadOrigen = @CiudadOrigen)
            """;

        const string sqlCount = """
            SELECT COUNT(*)
            FROM Equipos
            WHERE (@Nombre IS NULL OR Nombre LIKE '%' + @Nombre + '%')
              AND (@CiudadOrigen IS NULL OR CiudadOrigen = @CiudadOrigen)
            """;

        var ordenarPor = OrdenSql.Resolver(ColumnasOrdenables, consulta.OrdenarPor, consulta.DireccionOrden, "Nombre");

        using var conexion = _conexionFactory.CrearConexion();
        return await PaginadorDapper.ObtenerPaginaAsync<EquipoDto>(
            conexion,
            sqlSelect,
            sqlCount,
            ordenarPor,
            new { consulta.Nombre, consulta.CiudadOrigen },
            consulta.NumeroPagina,
            consulta.TamanoPagina,
            cancellationToken);
    }

    public async Task<EquipoDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id, Nombre, CiudadOrigen FROM Equipos WHERE Id = @Id";

        using var conexion = _conexionFactory.CrearConexion();
        var comando = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        return await conexion.QuerySingleOrDefaultAsync<EquipoDto>(comando);
    }

    public async Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(1) FROM Equipos WHERE Id = @Id";

        using var conexion = _conexionFactory.CrearConexion();
        var comando = new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken);
        var cantidad = await conexion.ExecuteScalarAsync<int>(comando);
        return cantidad > 0;
    }
}

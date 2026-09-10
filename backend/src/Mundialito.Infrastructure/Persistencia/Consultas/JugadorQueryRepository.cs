using Mundialito.Application.Comun;
using Mundialito.Application.Jugadores.Consultas;

namespace Mundialito.Infrastructure.Persistencia.Consultas;

public class JugadorQueryRepository : IJugadorQueryRepository
{
    private static readonly Dictionary<string, string> ColumnasOrdenables = new(StringComparer.OrdinalIgnoreCase)
    {
        ["nombre"] = "Nombre",
        ["posicion"] = "Posicion"
    };

    private readonly IConexionFactory _conexionFactory;

    public JugadorQueryRepository(IConexionFactory conexionFactory)
    {
        _conexionFactory = conexionFactory;
    }

    public async Task<PagedResult<JugadorDto>> ObtenerPorEquipoAsync(ObtenerJugadoresPorEquipoQuery consulta, CancellationToken cancellationToken)
    {
        const string sqlSelect = """
            SELECT Id, EquipoId, Nombre, Posicion
            FROM Jugadores
            WHERE EquipoId = @EquipoId
              AND (@Nombre IS NULL OR Nombre LIKE '%' + @Nombre + '%')
              AND (@Posicion IS NULL OR Posicion = @Posicion)
            """;

        const string sqlCount = """
            SELECT COUNT(*)
            FROM Jugadores
            WHERE EquipoId = @EquipoId
              AND (@Nombre IS NULL OR Nombre LIKE '%' + @Nombre + '%')
              AND (@Posicion IS NULL OR Posicion = @Posicion)
            """;

        var ordenarPor = OrdenSql.Resolver(ColumnasOrdenables, consulta.OrdenarPor, consulta.DireccionOrden, "Nombre");

        using var conexion = _conexionFactory.CrearConexion();
        return await PaginadorDapper.ObtenerPaginaAsync<JugadorDto>(
            conexion,
            sqlSelect,
            sqlCount,
            ordenarPor,
            new { consulta.EquipoId, consulta.Nombre, Posicion = (int?)consulta.Posicion },
            consulta.NumeroPagina,
            consulta.TamanoPagina,
            cancellationToken);
    }
}

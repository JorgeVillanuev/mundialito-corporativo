using Dapper;
using Mundialito.Infrastructure.Persistencia.Consultas;

namespace Mundialito.Infrastructure.Idempotencia;

public class IdempotenciaStore : IIdempotenciaStore
{
    private readonly IConexionFactory _conexionFactory;

    public IdempotenciaStore(IConexionFactory conexionFactory)
    {
        _conexionFactory = conexionFactory;
    }

    public async Task<RegistroIdempotencia?> BuscarAsync(string clave, string ruta, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, Clave, Ruta, HashPayload, CodigoEstadoRespuesta, CuerpoRespuesta, CreadoEn
            FROM RegistrosIdempotencia
            WHERE Clave = @Clave AND Ruta = @Ruta
            """;

        using var conexion = _conexionFactory.CrearConexion();

        var comando = new CommandDefinition(sql, new { Clave = clave, Ruta = ruta }, cancellationToken: cancellationToken);
        return await conexion.QuerySingleOrDefaultAsync<RegistroIdempotencia>(comando);
    }

    public async Task GuardarAsync(RegistroIdempotencia registro, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO RegistrosIdempotencia (Id, Clave, Ruta, HashPayload, CodigoEstadoRespuesta, CuerpoRespuesta, CreadoEn)
            VALUES (@Id, @Clave, @Ruta, @HashPayload, @CodigoEstadoRespuesta, @CuerpoRespuesta, @CreadoEn)
            """;

        using var conexion = _conexionFactory.CrearConexion();
        var comando = new CommandDefinition(sql, registro, cancellationToken: cancellationToken);
        await conexion.ExecuteAsync(comando);
    }
}

using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Mundialito.Infrastructure.Persistencia.Consultas;

public class SqlConexionFactory : IConexionFactory
{
    private readonly string _cadenaConexion;

    public SqlConexionFactory(IConfiguration configuracion)
    {
        _cadenaConexion = configuracion.GetConnectionString("Default")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'Default'.");
    }

    public IDbConnection CrearConexion() => new SqlConnection(_cadenaConexion);
}

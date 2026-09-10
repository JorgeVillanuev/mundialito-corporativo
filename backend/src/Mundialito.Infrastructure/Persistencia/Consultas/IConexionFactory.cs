using System.Data;

namespace Mundialito.Infrastructure.Persistencia.Consultas;

public interface IConexionFactory
{
    IDbConnection CrearConexion();
}

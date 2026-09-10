using System.Data;
using Dapper;
using Mundialito.Application.Comun;

namespace Mundialito.Infrastructure.Persistencia.Consultas;

public static class PaginadorDapper
{
    public static async Task<PagedResult<T>> ObtenerPaginaAsync<T>(
        IDbConnection conexion,
        string sqlSelect,
        string sqlCount,
        string ordenarPor,
        object parametros,
        int numeroPagina,
        int tamanoPagina,
        CancellationToken cancellationToken)
    {
        var sql = $"""
            {sqlSelect}
            ORDER BY {ordenarPor}
            OFFSET @Offset ROWS FETCH NEXT @TamanoPagina ROWS ONLY;
            {sqlCount}
            """;

        var parametrosDinamicos = new DynamicParameters(parametros);
        parametrosDinamicos.Add("Offset", (numeroPagina - 1) * tamanoPagina);
        parametrosDinamicos.Add("TamanoPagina", tamanoPagina);

        var comando = new CommandDefinition(sql, parametrosDinamicos, cancellationToken: cancellationToken);
        using var multi = await conexion.QueryMultipleAsync(comando);

        var elementos = (await multi.ReadAsync<T>()).ToList();
        var totalRegistros = await multi.ReadSingleAsync<int>();

        return new PagedResult<T>(elementos, numeroPagina, tamanoPagina, totalRegistros);
    }
}

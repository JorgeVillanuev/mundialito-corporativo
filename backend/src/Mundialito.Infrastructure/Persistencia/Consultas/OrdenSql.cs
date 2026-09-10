namespace Mundialito.Infrastructure.Persistencia.Consultas;

public static class OrdenSql
{
    public static string Resolver(
        IReadOnlyDictionary<string, string> columnasOrdenables,
        string? ordenarPor,
        string? direccionOrden,
        string columnaPorDefecto,
        bool descendentePorDefecto = false)
    {
        var columna = columnasOrdenables.TryGetValue(ordenarPor ?? string.Empty, out var columnaValida)
            ? columnaValida
            : columnaPorDefecto;

        var pidioDesc = string.Equals(direccionOrden, "desc", StringComparison.OrdinalIgnoreCase);
        var pidioAsc = string.Equals(direccionOrden, "asc", StringComparison.OrdinalIgnoreCase);
        var descendente = pidioDesc || (descendentePorDefecto && !pidioAsc);

        return $"{columna} {(descendente ? "DESC" : "ASC")}";
    }
}

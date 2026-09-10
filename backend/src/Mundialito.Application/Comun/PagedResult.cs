namespace Mundialito.Application.Comun;

public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Elementos { get; }

    public int NumeroPagina { get; }

    public int TamanoPagina { get; }

    public int TotalRegistros { get; }

    public int TotalPaginas => TamanoPagina == 0 ? 0 : (int)Math.Ceiling(TotalRegistros / (double)TamanoPagina);

    public PagedResult(IReadOnlyList<T> elementos, int numeroPagina, int tamanoPagina, int totalRegistros)
    {
        Elementos = elementos;
        NumeroPagina = numeroPagina;
        TamanoPagina = tamanoPagina;
        TotalRegistros = totalRegistros;
    }
}

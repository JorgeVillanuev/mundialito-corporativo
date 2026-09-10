namespace Mundialito.Infrastructure.Idempotencia;

public enum TipoResultadoIdempotencia
{
    Nuevo,
    RespuestaOriginal,
    Conflicto
}

public sealed class ResultadoEvaluacionIdempotencia
{
    public TipoResultadoIdempotencia Tipo { get; }

    public int? CodigoEstadoRespuestaOriginal { get; }

    public string? CuerpoRespuestaOriginal { get; }

    private ResultadoEvaluacionIdempotencia(TipoResultadoIdempotencia tipo, int? codigoEstado, string? cuerpo)
    {
        Tipo = tipo;
        CodigoEstadoRespuestaOriginal = codigoEstado;
        CuerpoRespuestaOriginal = cuerpo;
    }

    public static ResultadoEvaluacionIdempotencia Nuevo() => new(TipoResultadoIdempotencia.Nuevo, null, null);

    public static ResultadoEvaluacionIdempotencia RespuestaOriginal(int codigoEstado, string cuerpo) =>
        new(TipoResultadoIdempotencia.RespuestaOriginal, codigoEstado, cuerpo);

    public static ResultadoEvaluacionIdempotencia Conflicto() => new(TipoResultadoIdempotencia.Conflicto, null, null);
}

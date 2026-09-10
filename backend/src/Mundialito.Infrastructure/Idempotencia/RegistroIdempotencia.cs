namespace Mundialito.Infrastructure.Idempotencia;

public class RegistroIdempotencia
{
    public Guid Id { get; set; }

    public string Clave { get; set; } = string.Empty;

    public string Ruta { get; set; } = string.Empty;

    public string HashPayload { get; set; } = string.Empty;

    public int CodigoEstadoRespuesta { get; set; }

    public string CuerpoRespuesta { get; set; } = string.Empty;

    public DateTime CreadoEn { get; set; }
}

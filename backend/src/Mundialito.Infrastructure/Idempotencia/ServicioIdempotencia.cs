using System.Security.Cryptography;
using System.Text;

namespace Mundialito.Infrastructure.Idempotencia;

public class ServicioIdempotencia
{
    private readonly IIdempotenciaStore _idempotenciaStore;

    public ServicioIdempotencia(IIdempotenciaStore idempotenciaStore)
    {
        _idempotenciaStore = idempotenciaStore;
    }

    public async Task<ResultadoEvaluacionIdempotencia> EvaluarAsync(string clave, string ruta, string payload, CancellationToken cancellationToken)
    {
        var hashPayload = CalcularHash(payload);
        var registroExistente = await _idempotenciaStore.BuscarAsync(clave, ruta, cancellationToken);

        if (registroExistente is null)
            return ResultadoEvaluacionIdempotencia.Nuevo();

        if (registroExistente.HashPayload != hashPayload)
            return ResultadoEvaluacionIdempotencia.Conflicto();

        return ResultadoEvaluacionIdempotencia.RespuestaOriginal(registroExistente.CodigoEstadoRespuesta, registroExistente.CuerpoRespuesta);
    }

    public Task RegistrarRespuestaAsync(string clave, string ruta, string payload, int codigoEstadoRespuesta, string cuerpoRespuesta, CancellationToken cancellationToken)
    {
        var registro = new RegistroIdempotencia
        {
            Id = Guid.NewGuid(),
            Clave = clave,
            Ruta = ruta,
            HashPayload = CalcularHash(payload),
            CodigoEstadoRespuesta = codigoEstadoRespuesta,
            CuerpoRespuesta = cuerpoRespuesta,
            CreadoEn = DateTime.UtcNow
        };

        return _idempotenciaStore.GuardarAsync(registro, cancellationToken);
    }

    private static string CalcularHash(string payload)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes);
    }
}

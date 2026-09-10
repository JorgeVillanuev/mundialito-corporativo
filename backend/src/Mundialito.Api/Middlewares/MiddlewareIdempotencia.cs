using Mundialito.Infrastructure.Idempotencia;

namespace Mundialito.Api.Middlewares;

public class MiddlewareIdempotencia
{
    private const string EncabezadoIdempotencyKey = "Idempotency-Key";

    private readonly RequestDelegate _siguiente;

    public MiddlewareIdempotencia(RequestDelegate siguiente)
    {
        _siguiente = siguiente;
    }

    public async Task InvokeAsync(HttpContext context, ServicioIdempotencia servicioIdempotencia)
    {
        if (!HttpMethods.IsPost(context.Request.Method))
        {
            await _siguiente(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(EncabezadoIdempotencyKey, out var clave) || string.IsNullOrWhiteSpace(clave))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                codigoError = "IDEMPOTENCY_KEY_REQUERIDO",
                mensaje = "El header Idempotency-Key es obligatorio en este endpoint.",
                traceId = context.TraceIdentifier
            });
            return;
        }

        context.Request.EnableBuffering();
        using var lector = new StreamReader(context.Request.Body, leaveOpen: true);
        var payload = await lector.ReadToEndAsync();
        context.Request.Body.Position = 0;

        var ruta = context.Request.Path.Value ?? string.Empty;
        var resultado = await servicioIdempotencia.EvaluarAsync(clave!, ruta, payload, context.RequestAborted);

        switch (resultado.Tipo)
        {
            case TipoResultadoIdempotencia.RespuestaOriginal:
                context.Response.StatusCode = resultado.CodigoEstadoRespuestaOriginal!.Value;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(resultado.CuerpoRespuestaOriginal!);
                return;

            case TipoResultadoIdempotencia.Conflicto:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                await context.Response.WriteAsJsonAsync(new
                {
                    codigoError = "IDEMPOTENCY_KEY_CONFLICTO",
                    mensaje = "La misma Idempotency-Key se usó con un payload distinto.",
                    traceId = context.TraceIdentifier
                });
                return;

            default:
                var cuerpoOriginal = context.Response.Body;
                using (var bufferRespuesta = new MemoryStream())
                {
                    context.Response.Body = bufferRespuesta;

                    await _siguiente(context);

                    bufferRespuesta.Position = 0;
                    var cuerpoRespuesta = await new StreamReader(bufferRespuesta).ReadToEndAsync();

                    if (context.Response.StatusCode is >= 200 and < 300)
                    {
                        await servicioIdempotencia.RegistrarRespuestaAsync(
                            clave!, ruta, payload, context.Response.StatusCode, cuerpoRespuesta, context.RequestAborted);
                    }

                    bufferRespuesta.Position = 0;
                    await bufferRespuesta.CopyToAsync(cuerpoOriginal);
                }
                context.Response.Body = cuerpoOriginal;
                return;
        }
    }
}

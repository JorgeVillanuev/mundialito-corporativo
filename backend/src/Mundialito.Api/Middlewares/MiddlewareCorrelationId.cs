using System.Diagnostics;
using Serilog.Context;

namespace Mundialito.Api.Middlewares;

public class MiddlewareCorrelationId
{
    private const string EncabezadoCorrelationId = "X-Correlation-Id";

    private readonly RequestDelegate _siguiente;

    private readonly ILogger<MiddlewareCorrelationId> _logger;

    public MiddlewareCorrelationId(RequestDelegate siguiente, ILogger<MiddlewareCorrelationId> logger)
    {
        _siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(EncabezadoCorrelationId, out var valor) && !string.IsNullOrWhiteSpace(valor)
            ? valor.ToString()
            : Guid.NewGuid().ToString();

        context.Response.Headers[EncabezadoCorrelationId] = correlationId;

        using (LogContext.PushProperty("TraceId", correlationId))
        {
            var cronometro = Stopwatch.StartNew();

            await _siguiente(context);

            cronometro.Stop();
            _logger.LogInformation(
                "Request {Metodo} {Ruta} respondió {CodigoEstado} en {DuracionMs} ms",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                cronometro.ElapsedMilliseconds);
        }
    }
}

using System.Net;
using System.Text.Json;

namespace Mundialito.Api.Middlewares;

public class MiddlewareManejoErrores
{
    private readonly RequestDelegate _siguiente;

    private readonly ILogger<MiddlewareManejoErrores> _logger;

    public MiddlewareManejoErrores(RequestDelegate siguiente, ILogger<MiddlewareManejoErrores> logger)
    {
        _siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _siguiente(context);
        }
        catch (Exception excepcion)
        {
            _logger.LogError(excepcion, "Excepción no controlada procesando {Metodo} {Ruta}", context.Request.Method, context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var cuerpo = JsonSerializer.Serialize(new
            {
                codigoError = "ERROR_INTERNO",
                mensaje = "Ocurrió un error inesperado.",
                traceId = context.TraceIdentifier
            });

            await context.Response.WriteAsync(cuerpo);
        }
    }
}

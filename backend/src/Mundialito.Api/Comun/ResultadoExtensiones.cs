using Microsoft.AspNetCore.Mvc;
using Mundialito.Application.Comun;

namespace Mundialito.Api.Comun;

public static class ResultadoExtensiones
{
    public static IActionResult AActionResult(this Result resultado, ControllerBase controller)
    {
        return resultado.EsExitoso
            ? controller.NoContent()
            : ConstruirRespuestaError(resultado, controller);
    }

    public static IActionResult AActionResult<T>(this Result<T> resultado, ControllerBase controller, Func<T, IActionResult> alExito)
    {
        return resultado.EsExitoso
            ? alExito(resultado.Valor!)
            : ConstruirRespuestaError(resultado, controller);
    }

    private static IActionResult ConstruirRespuestaError(Result resultado, ControllerBase controller)
    {
        var cuerpo = new
        {
            codigoError = resultado.CodigoError,
            mensaje = resultado.MensajeError,
            traceId = controller.HttpContext.TraceIdentifier
        };

        return new ObjectResult(cuerpo) { StatusCode = ResolverCodigoEstado(resultado.CodigoError!) };
    }

    private static int ResolverCodigoEstado(string codigoError)
    {
        if (codigoError.EndsWith("_NO_ENCONTRADO", StringComparison.Ordinal))
            return StatusCodes.Status404NotFound;

        if (codigoError.EndsWith("_DUPLICADO", StringComparison.Ordinal) || codigoError.EndsWith("_CONFLICTO", StringComparison.Ordinal))
            return StatusCodes.Status409Conflict;

        return StatusCodes.Status400BadRequest;
    }
}

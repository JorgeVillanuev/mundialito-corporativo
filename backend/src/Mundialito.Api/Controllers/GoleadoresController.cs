using Microsoft.AspNetCore.Mvc;
using Mundialito.Application.Comun;
using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Api.Controllers;

[ApiController]
[Route("api/v1/goleadores")]
public class GoleadoresController : ControllerBase
{
    private readonly IQueryHandler<ObtenerGoleadoresQuery, PagedResult<GoleadorDto>> _obtenerHandler;

    public GoleadoresController(IQueryHandler<ObtenerGoleadoresQuery, PagedResult<GoleadorDto>> obtenerHandler)
    {
        _obtenerHandler = obtenerHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Obtener([FromQuery] ObtenerGoleadoresQuery consulta, CancellationToken cancellationToken)
    {
        var resultado = await _obtenerHandler.Manejar(consulta, cancellationToken);
        return Ok(resultado);
    }
}

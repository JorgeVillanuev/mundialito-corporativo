using Microsoft.AspNetCore.Mvc;
using Mundialito.Application.Comun;
using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Api.Controllers;

[ApiController]
[Route("api/v1/posiciones")]
public class PosicionesController : ControllerBase
{
    private readonly IQueryHandler<ObtenerPosicionesQuery, PagedResult<PosicionDto>> _obtenerHandler;

    public PosicionesController(IQueryHandler<ObtenerPosicionesQuery, PagedResult<PosicionDto>> obtenerHandler)
    {
        _obtenerHandler = obtenerHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Obtener([FromQuery] ObtenerPosicionesQuery consulta, CancellationToken cancellationToken)
    {
        var resultado = await _obtenerHandler.Manejar(consulta, cancellationToken);
        return Ok(resultado);
    }
}

using Microsoft.AspNetCore.Mvc;
using Mundialito.Api.Comun;
using Mundialito.Application.Comun;
using Mundialito.Application.Equipos.Comandos;
using Mundialito.Application.Equipos.Consultas;

namespace Mundialito.Api.Controllers;

[ApiController]
[Route("api/v1/equipos")]
public class EquiposController : ControllerBase
{
    private readonly ICommandHandler<CrearEquipoCommand, Result<Guid>> _crearHandler;
    private readonly ICommandHandler<ActualizarEquipoCommand, Result> _actualizarHandler;
    private readonly ICommandHandler<EliminarEquipoCommand, Result> _eliminarHandler;
    private readonly IQueryHandler<ObtenerEquiposQuery, PagedResult<EquipoDto>> _obtenerListaHandler;
    private readonly IQueryHandler<ObtenerEquipoPorIdQuery, Result<EquipoDto>> _obtenerPorIdHandler;

    public EquiposController(
        ICommandHandler<CrearEquipoCommand, Result<Guid>> crearHandler,
        ICommandHandler<ActualizarEquipoCommand, Result> actualizarHandler,
        ICommandHandler<EliminarEquipoCommand, Result> eliminarHandler,
        IQueryHandler<ObtenerEquiposQuery, PagedResult<EquipoDto>> obtenerListaHandler,
        IQueryHandler<ObtenerEquipoPorIdQuery, Result<EquipoDto>> obtenerPorIdHandler)
    {
        _crearHandler = crearHandler;
        _actualizarHandler = actualizarHandler;
        _eliminarHandler = eliminarHandler;
        _obtenerListaHandler = obtenerListaHandler;
        _obtenerPorIdHandler = obtenerPorIdHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearEquipoCommand comando, CancellationToken cancellationToken)
    {
        var resultado = await _crearHandler.Manejar(comando, cancellationToken);
        return resultado.AActionResult(this, id => CreatedAtAction(nameof(ObtenerPorId), new { id }, new { id }));
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerLista([FromQuery] ObtenerEquiposQuery consulta, CancellationToken cancellationToken)
    {
        var resultado = await _obtenerListaHandler.Manejar(consulta, cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _obtenerPorIdHandler.Manejar(new ObtenerEquipoPorIdQuery(id), cancellationToken);
        return resultado.AActionResult(this, dto => Ok(dto));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarEquipoRequest cuerpo, CancellationToken cancellationToken)
    {
        var comando = new ActualizarEquipoCommand(id, cuerpo.Nombre, cuerpo.CiudadOrigen);
        var resultado = await _actualizarHandler.Manejar(comando, cancellationToken);
        return resultado.AActionResult(this);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Eliminar(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _eliminarHandler.Manejar(new EliminarEquipoCommand(id), cancellationToken);
        return resultado.AActionResult(this);
    }
}

public sealed record ActualizarEquipoRequest(string Nombre, string CiudadOrigen);

using Microsoft.AspNetCore.Mvc;
using Mundialito.Api.Comun;
using Mundialito.Application.Comun;
using Mundialito.Application.Partidos.Comandos;
using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.Api.Controllers;

[ApiController]
[Route("api/v1/partidos")]
public class PartidosController : ControllerBase
{
    private readonly ICommandHandler<CrearPartidoCommand, Result<Guid>> _crearHandler;
    private readonly ICommandHandler<RegistrarResultadoPartidoCommand, Result> _registrarResultadoHandler;
    private readonly IQueryHandler<ObtenerPartidosQuery, PagedResult<PartidoDto>> _obtenerListaHandler;
    private readonly IQueryHandler<ObtenerPartidoPorIdQuery, Result<PartidoDto>> _obtenerPorIdHandler;

    public PartidosController(
        ICommandHandler<CrearPartidoCommand, Result<Guid>> crearHandler,
        ICommandHandler<RegistrarResultadoPartidoCommand, Result> registrarResultadoHandler,
        IQueryHandler<ObtenerPartidosQuery, PagedResult<PartidoDto>> obtenerListaHandler,
        IQueryHandler<ObtenerPartidoPorIdQuery, Result<PartidoDto>> obtenerPorIdHandler)
    {
        _crearHandler = crearHandler;
        _registrarResultadoHandler = registrarResultadoHandler;
        _obtenerListaHandler = obtenerListaHandler;
        _obtenerPorIdHandler = obtenerPorIdHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearPartidoCommand comando, CancellationToken cancellationToken)
    {
        var resultado = await _crearHandler.Manejar(comando, cancellationToken);
        return resultado.AActionResult(this, id => CreatedAtAction(nameof(ObtenerPorId), new { id }, new { id }));
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerLista([FromQuery] ObtenerPartidosQuery consulta, CancellationToken cancellationToken)
    {
        var resultado = await _obtenerListaHandler.Manejar(consulta, cancellationToken);
        return Ok(resultado);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var resultado = await _obtenerPorIdHandler.Manejar(new ObtenerPartidoPorIdQuery(id), cancellationToken);
        return resultado.AActionResult(this, dto => Ok(dto));
    }

    [HttpPut("{id:guid}/resultado")]
    public async Task<IActionResult> RegistrarResultado(Guid id, [FromBody] RegistrarResultadoPartidoRequest cuerpo, CancellationToken cancellationToken)
    {
        var comando = new RegistrarResultadoPartidoCommand(id, cuerpo.GolesLocal, cuerpo.GolesVisitante, cuerpo.Goleadores);
        var resultado = await _registrarResultadoHandler.Manejar(comando, cancellationToken);
        var r = resultado.AActionResult(this);
        return r;
    }
}

public sealed record RegistrarResultadoPartidoRequest(int GolesLocal, int GolesVisitante, IReadOnlyList<GolJugadorEntrada> Goleadores);

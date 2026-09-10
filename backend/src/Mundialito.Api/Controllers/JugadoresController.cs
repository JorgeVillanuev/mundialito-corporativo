using Microsoft.AspNetCore.Mvc;
using Mundialito.Api.Comun;
using Mundialito.Application.Comun;
using Mundialito.Application.Jugadores.Comandos;
using Mundialito.Application.Jugadores.Consultas;
using Mundialito.Domain.Enums;

namespace Mundialito.Api.Controllers;

[ApiController]
[Route("api/v1/equipos/{equipoId:guid}/jugadores")]
public class JugadoresController : ControllerBase
{
    private readonly ICommandHandler<RegistrarJugadorCommand, Result<Guid>> _registrarHandler;
    private readonly IQueryHandler<ObtenerJugadoresPorEquipoQuery, Result<PagedResult<JugadorDto>>> _obtenerListaHandler;

    public JugadoresController(
        ICommandHandler<RegistrarJugadorCommand, Result<Guid>> registrarHandler,
        IQueryHandler<ObtenerJugadoresPorEquipoQuery, Result<PagedResult<JugadorDto>>> obtenerListaHandler)
    {
        _registrarHandler = registrarHandler;
        _obtenerListaHandler = obtenerListaHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar(Guid equipoId, [FromBody] RegistrarJugadorRequest cuerpo, CancellationToken cancellationToken)
    {
        var comando = new RegistrarJugadorCommand(equipoId, cuerpo.Nombre, cuerpo.Posicion);
        var resultado = await _registrarHandler.Manejar(comando, cancellationToken);
        return resultado.AActionResult(this, id => StatusCode(StatusCodes.Status201Created, new { id }));
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerLista(Guid equipoId, [FromQuery] FiltrosJugadoresRequest filtros, CancellationToken cancellationToken)
    {
        var consulta = new ObtenerJugadoresPorEquipoQuery(
            equipoId, filtros.Nombre, filtros.Posicion, filtros.NumeroPagina, filtros.TamanoPagina, filtros.OrdenarPor, filtros.DireccionOrden);

        var resultado = await _obtenerListaHandler.Manejar(consulta, cancellationToken);
        return resultado.AActionResult(this, paginado => Ok(paginado));
    }
}

public sealed record RegistrarJugadorRequest(string Nombre, PosicionJugador Posicion);

public sealed record FiltrosJugadoresRequest(
    string? Nombre = null,
    PosicionJugador? Posicion = null,
    int NumeroPagina = 1,
    int TamanoPagina = 10,
    string? OrdenarPor = null,
    string? DireccionOrden = null);

using Mundialito.Application.Comun;

namespace Mundialito.Application.Jugadores.Consultas;

public sealed class ObtenerJugadoresPorEquipoQueryHandler
    : IQueryHandler<ObtenerJugadoresPorEquipoQuery, Result<PagedResult<JugadorDto>>>
{
    private readonly IEquipoQueryRepository _equipoQueryRepository;

    private readonly IJugadorQueryRepository _jugadorQueryRepository;

    public ObtenerJugadoresPorEquipoQueryHandler(
        IEquipoQueryRepository equipoQueryRepository,
        IJugadorQueryRepository jugadorQueryRepository)
    {
        _equipoQueryRepository = equipoQueryRepository;
        _jugadorQueryRepository = jugadorQueryRepository;
    }

    public async Task<Result<PagedResult<JugadorDto>>> Manejar(ObtenerJugadoresPorEquipoQuery consulta, CancellationToken cancellationToken)
    {
        var existeEquipo = await _equipoQueryRepository.ExisteAsync(consulta.EquipoId, cancellationToken);
        if (!existeEquipo)
            return Result<PagedResult<JugadorDto>>.Falla(CodigosError.EquipoNoEncontrado, $"No existe un equipo con id '{consulta.EquipoId}'.");

        var resultado = await _jugadorQueryRepository.ObtenerPorEquipoAsync(consulta, cancellationToken);

        return Result<PagedResult<JugadorDto>>.Exito(resultado);
    }
}

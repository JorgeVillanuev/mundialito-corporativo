using Mundialito.Application.Comun;

namespace Mundialito.Application.Partidos.Consultas;

public sealed class ObtenerPosicionesQueryHandler : IQueryHandler<ObtenerPosicionesQuery, PagedResult<PosicionDto>>
{
    private readonly IPosicionesQueryRepository _posicionesQueryRepository;

    public ObtenerPosicionesQueryHandler(IPosicionesQueryRepository posicionesQueryRepository)
    {
        _posicionesQueryRepository = posicionesQueryRepository;
    }

    public Task<PagedResult<PosicionDto>> Manejar(ObtenerPosicionesQuery consulta, CancellationToken cancellationToken)
    {
        return _posicionesQueryRepository.ObtenerAsync(consulta, cancellationToken);
    }
}

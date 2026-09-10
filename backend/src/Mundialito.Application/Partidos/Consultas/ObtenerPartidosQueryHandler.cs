using Mundialito.Application.Comun;

namespace Mundialito.Application.Partidos.Consultas;

public sealed class ObtenerPartidosQueryHandler : IQueryHandler<ObtenerPartidosQuery, PagedResult<PartidoDto>>
{
    private readonly IPartidoQueryRepository _partidoQueryRepository;

    public ObtenerPartidosQueryHandler(IPartidoQueryRepository partidoQueryRepository)
    {
        _partidoQueryRepository = partidoQueryRepository;
    }

    public Task<PagedResult<PartidoDto>> Manejar(ObtenerPartidosQuery consulta, CancellationToken cancellationToken)
    {
        return _partidoQueryRepository.ObtenerAsync(consulta, cancellationToken);
    }
}

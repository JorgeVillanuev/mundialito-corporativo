using Mundialito.Application.Comun;

namespace Mundialito.Application.Partidos.Consultas;

public sealed class ObtenerGoleadoresQueryHandler : IQueryHandler<ObtenerGoleadoresQuery, PagedResult<GoleadorDto>>
{
    private readonly IGoleadoresQueryRepository _goleadoresQueryRepository;

    public ObtenerGoleadoresQueryHandler(IGoleadoresQueryRepository goleadoresQueryRepository)
    {
        _goleadoresQueryRepository = goleadoresQueryRepository;
    }

    public Task<PagedResult<GoleadorDto>> Manejar(ObtenerGoleadoresQuery consulta, CancellationToken cancellationToken)
    {
        return _goleadoresQueryRepository.ObtenerAsync(consulta, cancellationToken);
    }
}

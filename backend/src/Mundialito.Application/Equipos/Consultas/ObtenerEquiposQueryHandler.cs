using Mundialito.Application.Comun;

namespace Mundialito.Application.Equipos.Consultas;

public sealed class ObtenerEquiposQueryHandler : IQueryHandler<ObtenerEquiposQuery, PagedResult<EquipoDto>>
{
    private readonly IEquipoQueryRepository _equipoQueryRepository;

    public ObtenerEquiposQueryHandler(IEquipoQueryRepository equipoQueryRepository)
    {
        _equipoQueryRepository = equipoQueryRepository;
    }

    public Task<PagedResult<EquipoDto>> Manejar(ObtenerEquiposQuery consulta, CancellationToken cancellationToken)
    {
        return _equipoQueryRepository.ObtenerAsync(consulta, cancellationToken);
    }
}

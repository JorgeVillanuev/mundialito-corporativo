using Mundialito.Application.Comun;

namespace Mundialito.Application.Partidos.Consultas;

public sealed class ObtenerPartidoPorIdQueryHandler : IQueryHandler<ObtenerPartidoPorIdQuery, Result<PartidoDto>>
{
    private readonly IPartidoQueryRepository _partidoQueryRepository;

    public ObtenerPartidoPorIdQueryHandler(IPartidoQueryRepository partidoQueryRepository)
    {
        _partidoQueryRepository = partidoQueryRepository;
    }

    public async Task<Result<PartidoDto>> Manejar(ObtenerPartidoPorIdQuery consulta, CancellationToken cancellationToken)
    {
        var partido = await _partidoQueryRepository.ObtenerPorIdAsync(consulta.Id, cancellationToken);
        if (partido is null)
            return Result<PartidoDto>.Falla(CodigosError.PartidoNoEncontrado, $"No existe un partido con id '{consulta.Id}'.");

        return Result<PartidoDto>.Exito(partido);
    }
}

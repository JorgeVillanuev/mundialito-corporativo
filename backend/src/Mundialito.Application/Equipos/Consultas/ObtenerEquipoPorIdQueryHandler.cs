using Mundialito.Application.Comun;

namespace Mundialito.Application.Equipos.Consultas;

public sealed class ObtenerEquipoPorIdQueryHandler : IQueryHandler<ObtenerEquipoPorIdQuery, Result<EquipoDto>>
{
    private readonly IEquipoQueryRepository _equipoQueryRepository;

    public ObtenerEquipoPorIdQueryHandler(IEquipoQueryRepository equipoQueryRepository)
    {
        _equipoQueryRepository = equipoQueryRepository;
    }

    public async Task<Result<EquipoDto>> Manejar(ObtenerEquipoPorIdQuery consulta, CancellationToken cancellationToken)
    {
        var equipo = await _equipoQueryRepository.ObtenerPorIdAsync(consulta.Id, cancellationToken);
        if (equipo is null)
            return Result<EquipoDto>.Falla(CodigosError.EquipoNoEncontrado, $"No existe un equipo con id '{consulta.Id}'.");

        return Result<EquipoDto>.Exito(equipo);
    }
}

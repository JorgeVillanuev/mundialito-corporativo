using FluentValidation;
using Mundialito.Application.Comun;
using Mundialito.Domain.Entidades;

namespace Mundialito.Application.Partidos.Comandos;

public sealed class CrearPartidoCommandHandler : ICommandHandler<CrearPartidoCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CrearPartidoCommand> _validador;

    public CrearPartidoCommandHandler(IUnitOfWork unitOfWork, IValidator<CrearPartidoCommand> validador)
    {
        _unitOfWork = unitOfWork;
        _validador = validador;
    }

    public async Task<Result<Guid>> Manejar(CrearPartidoCommand comando, CancellationToken cancellationToken)
    {
        var errorValidacion = await Validaciones.ValidarAsync(_validador, comando, cancellationToken);
        if (errorValidacion is not null)
            return Result<Guid>.Falla(CodigosError.ValidacionFallida, errorValidacion);

        if (comando.EquipoLocalId == comando.EquipoVisitanteId)
            return Result<Guid>.Falla(CodigosError.PartidoEquiposIgualesConflicto, "El equipo local y el equipo visitante no pueden ser el mismo.");

        var equipoLocal = await _unitOfWork.Equipos.ObtenerPorIdAsync(comando.EquipoLocalId, cancellationToken);
        if (equipoLocal is null)
            return Result<Guid>.Falla(CodigosError.EquipoNoEncontrado, $"No existe un equipo local con id '{comando.EquipoLocalId}'.");

        var equipoVisitante = await _unitOfWork.Equipos.ObtenerPorIdAsync(comando.EquipoVisitanteId, cancellationToken);
        if (equipoVisitante is null)
            return Result<Guid>.Falla(CodigosError.EquipoNoEncontrado, $"No existe un equipo visitante con id '{comando.EquipoVisitanteId}'.");

        var partido = Partido.Crear(comando.EquipoLocalId, comando.EquipoVisitanteId, comando.FechaHora);
        _unitOfWork.Partidos.Agregar(partido);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result<Guid>.Exito(partido.Id);
    }
}

using FluentValidation;
using Mundialito.Application.Comun;

namespace Mundialito.Application.Equipos.Comandos;

public sealed class ActualizarEquipoCommandHandler : ICommandHandler<ActualizarEquipoCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ActualizarEquipoCommand> _validador;

    public ActualizarEquipoCommandHandler(IUnitOfWork unitOfWork, IValidator<ActualizarEquipoCommand> validador)
    {
        _unitOfWork = unitOfWork;
        _validador = validador;
    }

    public async Task<Result> Manejar(ActualizarEquipoCommand comando, CancellationToken cancellationToken)
    {
        var errorValidacion = await Validaciones.ValidarAsync(_validador, comando, cancellationToken);
        if (errorValidacion is not null)
            return Result.Falla(CodigosError.ValidacionFallida, errorValidacion);

        var equipo = await _unitOfWork.Equipos.ObtenerPorIdAsync(comando.Id, cancellationToken);
        if (equipo is null)
            return Result.Falla(CodigosError.EquipoNoEncontrado, $"No existe un equipo con id '{comando.Id}'.");

        var existeConMismoNombre = await _unitOfWork.Equipos.ExisteConNombreAsync(comando.Nombre, comando.Id, cancellationToken);
        if (existeConMismoNombre)
            return Result.Falla(CodigosError.EquipoNombreDuplicado, $"Ya existe un equipo con el nombre '{comando.Nombre}'.");

        equipo.Actualizar(comando.Nombre, comando.CiudadOrigen);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Exito();
    }
}

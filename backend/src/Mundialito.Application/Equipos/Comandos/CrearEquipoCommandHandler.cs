using FluentValidation;
using Mundialito.Application.Comun;
using Mundialito.Domain.Entidades;

namespace Mundialito.Application.Equipos.Comandos;

public sealed class CrearEquipoCommandHandler : ICommandHandler<CrearEquipoCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CrearEquipoCommand> _validador;

    public CrearEquipoCommandHandler(IUnitOfWork unitOfWork, IValidator<CrearEquipoCommand> validador)
    {
        _unitOfWork = unitOfWork;
        _validador = validador;
    }

    public async Task<Result<Guid>> Manejar(CrearEquipoCommand comando, CancellationToken cancellationToken)
    {
        var errorValidacion = await Validaciones.ValidarAsync(_validador, comando, cancellationToken);
        if (errorValidacion is not null)
            return Result<Guid>.Falla(CodigosError.ValidacionFallida, errorValidacion);

        var existeConMismoNombre = await _unitOfWork.Equipos.ExisteConNombreAsync(comando.Nombre, null, cancellationToken);
        if (existeConMismoNombre)
            return Result<Guid>.Falla(CodigosError.EquipoNombreDuplicado, $"Ya existe un equipo con el nombre '{comando.Nombre}'.");

        var equipo = Equipo.Crear(comando.Nombre, comando.CiudadOrigen);
        _unitOfWork.Equipos.Agregar(equipo);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result<Guid>.Exito(equipo.Id);
    }
}

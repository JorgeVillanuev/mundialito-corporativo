using FluentValidation;
using Mundialito.Application.Comun;
using Mundialito.Domain.Entidades;

namespace Mundialito.Application.Jugadores.Comandos;

public sealed class RegistrarJugadorCommandHandler : ICommandHandler<RegistrarJugadorCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegistrarJugadorCommand> _validador;

    public RegistrarJugadorCommandHandler(IUnitOfWork unitOfWork, IValidator<RegistrarJugadorCommand> validador)
    {
        _unitOfWork = unitOfWork;
        _validador = validador;
    }

    public async Task<Result<Guid>> Manejar(RegistrarJugadorCommand comando, CancellationToken cancellationToken)
    {
        var errorValidacion = await Validaciones.ValidarAsync(_validador, comando, cancellationToken);
        if (errorValidacion is not null)
            return Result<Guid>.Falla(CodigosError.ValidacionFallida, errorValidacion);

        var equipo = await _unitOfWork.Equipos.ObtenerPorIdAsync(comando.EquipoId, cancellationToken);
        if (equipo is null)
            return Result<Guid>.Falla(CodigosError.EquipoNoEncontrado, $"No existe un equipo con id '{comando.EquipoId}'.");

        var jugador = Jugador.Crear(comando.EquipoId, comando.Nombre, comando.Posicion);
        _unitOfWork.Jugadores.Agregar(jugador);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result<Guid>.Exito(jugador.Id);
    }
}

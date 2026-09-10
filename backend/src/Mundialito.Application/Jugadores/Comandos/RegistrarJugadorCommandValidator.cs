using FluentValidation;
using Mundialito.Domain.Entidades;

namespace Mundialito.Application.Jugadores.Comandos;

public sealed class RegistrarJugadorCommandValidator : AbstractValidator<RegistrarJugadorCommand>
{
    public RegistrarJugadorCommandValidator()
    {
        RuleFor(c => c.EquipoId).NotEmpty().WithMessage("El equipo es obligatorio.");

        RuleFor(c => c.Nombre)
            .NotEmpty().WithMessage("El nombre del jugador es obligatorio.")
            .MaximumLength(Jugador.LongitudMaximaNombre).WithMessage($"El nombre del jugador no puede superar {Jugador.LongitudMaximaNombre} caracteres.");

        RuleFor(c => c.Posicion).IsInEnum().WithMessage("La posición del jugador no es válida.");
    }
}

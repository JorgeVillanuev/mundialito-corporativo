using FluentValidation;

namespace Mundialito.Application.Partidos.Comandos;

public sealed class CrearPartidoCommandValidator : AbstractValidator<CrearPartidoCommand>
{
    public CrearPartidoCommandValidator()
    {
        RuleFor(c => c.EquipoLocalId).NotEmpty().WithMessage("El equipo local es obligatorio.");
        RuleFor(c => c.EquipoVisitanteId).NotEmpty().WithMessage("El equipo visitante es obligatorio.");

    }
}

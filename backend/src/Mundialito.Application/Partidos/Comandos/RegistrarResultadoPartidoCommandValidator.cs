using FluentValidation;

namespace Mundialito.Application.Partidos.Comandos;

public sealed class RegistrarResultadoPartidoCommandValidator : AbstractValidator<RegistrarResultadoPartidoCommand>
{
    public RegistrarResultadoPartidoCommandValidator()
    {
        RuleFor(c => c.PartidoId).NotEmpty().WithMessage("El partido es obligatorio.");
        RuleFor(c => c.GolesLocal).GreaterThanOrEqualTo(0).WithMessage("Los goles del equipo local no pueden ser negativos.");
        RuleFor(c => c.GolesVisitante).GreaterThanOrEqualTo(0).WithMessage("Los goles del equipo visitante no pueden ser negativos.");

        RuleForEach(c => c.Goleadores).ChildRules(goleador =>
        {
            goleador.RuleFor(g => g.JugadorId).NotEmpty().WithMessage("El jugador del gol es obligatorio.");
            goleador.RuleFor(g => g.CantidadGoles).GreaterThanOrEqualTo(1).WithMessage("La cantidad de goles de un jugador debe ser al menos 1.");
        });
    }
}

using FluentValidation;
using Mundialito.Domain.Entidades;

namespace Mundialito.Application.Comun;

public static class ValidacionesComunes
{
    public static IRuleBuilderOptions<T, string> NombreDeEquipoValido<T>(this IRuleBuilder<T, string> regla)
    {
        return regla
            .NotEmpty().WithMessage("El nombre del equipo es obligatorio.")
            .MaximumLength(Equipo.LongitudMaximaNombre)
            .WithMessage($"El nombre del equipo no puede superar {Equipo.LongitudMaximaNombre} caracteres.");
    }
}

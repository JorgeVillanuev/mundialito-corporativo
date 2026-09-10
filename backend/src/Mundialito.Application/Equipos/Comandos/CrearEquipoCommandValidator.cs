using FluentValidation;
using Mundialito.Application.Comun;

namespace Mundialito.Application.Equipos.Comandos;

public sealed class CrearEquipoCommandValidator : AbstractValidator<CrearEquipoCommand>
{
    public CrearEquipoCommandValidator()
    {
        RuleFor(c => c.Nombre).NombreDeEquipoValido();
        RuleFor(c => c.CiudadOrigen).NotEmpty().WithMessage("La ciudad de origen es obligatoria.");
    }
}

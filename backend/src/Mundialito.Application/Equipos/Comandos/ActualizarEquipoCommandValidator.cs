using FluentValidation;
using Mundialito.Application.Comun;

namespace Mundialito.Application.Equipos.Comandos;

public sealed class ActualizarEquipoCommandValidator : AbstractValidator<ActualizarEquipoCommand>
{
    public ActualizarEquipoCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty().WithMessage("El id del equipo es obligatorio.");
        RuleFor(c => c.Nombre).NombreDeEquipoValido();
        RuleFor(c => c.CiudadOrigen).NotEmpty().WithMessage("La ciudad de origen es obligatoria.");
    }
}

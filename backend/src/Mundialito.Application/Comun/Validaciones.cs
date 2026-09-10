using FluentValidation;

namespace Mundialito.Application.Comun;

public static class Validaciones
{
    public static async Task<string?> ValidarAsync<T>(IValidator<T> validador, T comando, CancellationToken cancellationToken)
    {
        var resultado = await validador.ValidateAsync(comando, cancellationToken);
        return resultado.IsValid ? null : string.Join(" ", resultado.Errors.Select(e => e.ErrorMessage));
    }
}

namespace Mundialito.Application.Comun;

public class Result
{
    public bool EsExitoso { get; }

    public bool EsFallido => !EsExitoso;

    public string? CodigoError { get; }

    public string? MensajeError { get; }

    protected Result(bool esExitoso, string? codigoError, string? mensajeError)
    {
        if (esExitoso && codigoError is not null)
            throw new InvalidOperationException("Un resultado exitoso no puede tener código de error.");
        if (!esExitoso && codigoError is null)
            throw new InvalidOperationException("Un resultado fallido debe tener código de error.");

        EsExitoso = esExitoso;
        CodigoError = codigoError;
        MensajeError = mensajeError;
    }

    public static Result Exito() => new(true, null, null);

    public static Result Falla(string codigoError, string mensajeError) => new(false, codigoError, mensajeError);
}

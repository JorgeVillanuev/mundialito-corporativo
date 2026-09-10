namespace Mundialito.Application.Comun;

public sealed class Result<T> : Result
{
    public T? Valor { get; }

    private Result(T valor) : base(true, null, null)
    {
        Valor = valor;
    }

    private Result(string codigoError, string mensajeError) : base(false, codigoError, mensajeError)
    {
        Valor = default;
    }

    public static Result<T> Exito(T valor) => new(valor);

    public static new Result<T> Falla(string codigoError, string mensajeError) => new(codigoError, mensajeError);
}

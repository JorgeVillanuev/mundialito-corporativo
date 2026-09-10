namespace Mundialito.Application.Comun;

public interface ICommandHandler<TComando, TResultado>
{
    Task<TResultado> Manejar(TComando comando, CancellationToken cancellationToken);
}

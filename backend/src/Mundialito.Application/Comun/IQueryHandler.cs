namespace Mundialito.Application.Comun;

public interface IQueryHandler<TConsulta, TResultado>
{
    Task<TResultado> Manejar(TConsulta consulta, CancellationToken cancellationToken);
}

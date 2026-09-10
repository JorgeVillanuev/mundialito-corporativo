namespace Mundialito.Infrastructure.Idempotencia;

public interface IIdempotenciaStore
{
    Task<RegistroIdempotencia?> BuscarAsync(string clave, string ruta, CancellationToken cancellationToken);

    Task GuardarAsync(RegistroIdempotencia registro, CancellationToken cancellationToken);
}

namespace Mundialito.Application.Comun;

public interface IUnitOfWork
{
    IEquipoRepository Equipos { get; }

    IJugadorRepository Jugadores { get; }

    IPartidoRepository Partidos { get; }

    Task<int> GuardarCambiosAsync(CancellationToken cancellationToken);
}

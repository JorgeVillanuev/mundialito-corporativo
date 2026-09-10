using Mundialito.Domain.Entidades;

namespace Mundialito.Application.Comun;

public interface IEquipoRepository
{
    Task<Equipo?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExisteConNombreAsync(string nombre, Guid? idAExcluir, CancellationToken cancellationToken);

    Task<bool> TieneDependenciasAsync(Guid equipoId, CancellationToken cancellationToken);

    void Agregar(Equipo equipo);

    void Eliminar(Equipo equipo);
}

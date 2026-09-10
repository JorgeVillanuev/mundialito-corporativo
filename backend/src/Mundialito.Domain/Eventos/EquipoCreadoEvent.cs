using Mundialito.Domain.Comun;

namespace Mundialito.Domain.Eventos;

public sealed class EquipoCreadoEvent : IDomainEvent
{
    public Guid EquipoId { get; }

    public string Nombre { get; }

    public DateTime OcurrioEn { get; }

    public EquipoCreadoEvent(Guid equipoId, string nombre)
    {
        EquipoId = equipoId;
        Nombre = nombre;
        OcurrioEn = DateTime.UtcNow;
    }
}

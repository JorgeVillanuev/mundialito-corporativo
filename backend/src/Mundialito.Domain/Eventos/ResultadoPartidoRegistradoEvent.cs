using Mundialito.Domain.Comun;

namespace Mundialito.Domain.Eventos;

public sealed class ResultadoPartidoRegistradoEvent : IDomainEvent
{
    public Guid PartidoId { get; }

    public int GolesLocal { get; }

    public int GolesVisitante { get; }

    public DateTime OcurrioEn { get; }

    public ResultadoPartidoRegistradoEvent(Guid partidoId, int golesLocal, int golesVisitante)
    {
        PartidoId = partidoId;
        GolesLocal = golesLocal;
        GolesVisitante = golesVisitante;
        OcurrioEn = DateTime.UtcNow;
    }
}

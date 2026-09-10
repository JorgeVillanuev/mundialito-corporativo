using Mundialito.Domain.Comun;
using Mundialito.Domain.Enums;
using Mundialito.Domain.Eventos;

namespace Mundialito.Domain.Entidades;

public class Partido : Entity
{
    public Guid EquipoLocalId { get; private set; }

    public Guid EquipoVisitanteId { get; private set; }

    public DateTime FechaHora { get; private set; }

    public EstadoPartido Estado { get; private set; }

    public int? GolesLocal { get; private set; }

    public int? GolesVisitante { get; private set; }

    public DateTime? FechaResultadoRegistrado { get; private set; }

    private Partido() { }

    public static Partido Crear(Guid equipoLocalId, Guid equipoVisitanteId, DateTime fechaHora)
    {
        if (equipoLocalId == Guid.Empty)
            throw new ArgumentException("El equipo local es obligatorio.", nameof(equipoLocalId));
        if (equipoVisitanteId == Guid.Empty)
            throw new ArgumentException("El equipo visitante es obligatorio.", nameof(equipoVisitanteId));

        return new Partido
        {
            EquipoLocalId = equipoLocalId,
            EquipoVisitanteId = equipoVisitanteId,
            FechaHora = fechaHora,
            Estado = EstadoPartido.Programado
        };
    }

    public void RegistrarResultado(int golesLocal, int golesVisitante)
    {
        if (golesLocal < 0)
            throw new ArgumentOutOfRangeException(nameof(golesLocal), "Los goles del equipo local no pueden ser negativos.");
        if (golesVisitante < 0)
            throw new ArgumentOutOfRangeException(nameof(golesVisitante), "Los goles del equipo visitante no pueden ser negativos.");

        GolesLocal = golesLocal;
        GolesVisitante = golesVisitante;
        Estado = EstadoPartido.Jugado;
        FechaResultadoRegistrado = DateTime.UtcNow;

        AgregarEvento(new ResultadoPartidoRegistradoEvent(Id, golesLocal, golesVisitante));
    }
}

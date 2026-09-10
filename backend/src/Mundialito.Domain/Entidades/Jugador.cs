using Mundialito.Domain.Comun;
using Mundialito.Domain.Enums;

namespace Mundialito.Domain.Entidades;

public class Jugador : Entity
{
    public const int LongitudMaximaNombre = 100;

    public Guid EquipoId { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public PosicionJugador Posicion { get; private set; }

    private Jugador() { }

    public static Jugador Crear(Guid equipoId, string nombre, PosicionJugador posicion)
    {
        if (equipoId == Guid.Empty)
            throw new ArgumentException("El equipo del jugador es obligatorio.", nameof(equipoId));
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del jugador es obligatorio.", nameof(nombre));
        if (nombre.Length > LongitudMaximaNombre)
            throw new ArgumentException($"El nombre del jugador no puede superar {LongitudMaximaNombre} caracteres.", nameof(nombre));

        return new Jugador
        {
            EquipoId = equipoId,
            Nombre = nombre,
            Posicion = posicion
        };
    }
}

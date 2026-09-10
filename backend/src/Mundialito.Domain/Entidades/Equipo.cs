using Mundialito.Domain.Comun;
using Mundialito.Domain.Eventos;

namespace Mundialito.Domain.Entidades;

public class Equipo : Entity
{
    public const int LongitudMaximaNombre = 100;

    public string Nombre { get; private set; } = string.Empty;

    public string CiudadOrigen { get; private set; } = string.Empty;

    private Equipo() { }

    public static Equipo Crear(string nombre, string ciudadOrigen)
    {
        ValidarNombre(nombre);
        if (string.IsNullOrWhiteSpace(ciudadOrigen))
            throw new ArgumentException("La ciudad de origen es obligatoria.", nameof(ciudadOrigen));

        var equipo = new Equipo
        {
            Nombre = nombre,
            CiudadOrigen = ciudadOrigen
        };

        equipo.AgregarEvento(new EquipoCreadoEvent(equipo.Id, equipo.Nombre));

        return equipo;
    }

    public void Actualizar(string nombre, string ciudadOrigen)
    {
        ValidarNombre(nombre);
        if (string.IsNullOrWhiteSpace(ciudadOrigen))
            throw new ArgumentException("La ciudad de origen es obligatoria.", nameof(ciudadOrigen));

        Nombre = nombre;
        CiudadOrigen = ciudadOrigen;
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del equipo es obligatorio.", nameof(nombre));
        if (nombre.Length > LongitudMaximaNombre)
            throw new ArgumentException($"El nombre del equipo no puede superar {LongitudMaximaNombre} caracteres.", nameof(nombre));
    }
}

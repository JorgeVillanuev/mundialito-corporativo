namespace Mundialito.Application.Equipos.Comandos;

public sealed record CrearEquipoCommand(
    string Nombre,
    string CiudadOrigen
);

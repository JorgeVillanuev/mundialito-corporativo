namespace Mundialito.Application.Equipos.Comandos;

public sealed record ActualizarEquipoCommand(
    Guid Id,
    string Nombre,
    string CiudadOrigen
);

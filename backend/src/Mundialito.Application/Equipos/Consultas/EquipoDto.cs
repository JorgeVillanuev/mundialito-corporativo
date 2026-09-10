namespace Mundialito.Application.Equipos.Consultas;

public sealed class EquipoDto
{
    public Guid Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string CiudadOrigen { get; init; } = string.Empty;
}

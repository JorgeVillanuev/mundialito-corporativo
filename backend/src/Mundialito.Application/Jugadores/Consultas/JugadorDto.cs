using Mundialito.Domain.Enums;

namespace Mundialito.Application.Jugadores.Consultas;

public sealed class JugadorDto
{
    public Guid Id { get; init; }

    public Guid EquipoId { get; init; }

    public string Nombre { get; init; } = string.Empty;

    public PosicionJugador Posicion { get; init; }
}

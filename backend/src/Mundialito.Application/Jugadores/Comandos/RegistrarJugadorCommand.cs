using Mundialito.Domain.Enums;

namespace Mundialito.Application.Jugadores.Comandos;

public sealed record RegistrarJugadorCommand(
    Guid EquipoId,
    string Nombre,
    PosicionJugador Posicion);

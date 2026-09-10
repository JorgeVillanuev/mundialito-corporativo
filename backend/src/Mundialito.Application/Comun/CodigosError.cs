namespace Mundialito.Application.Comun;

public static class CodigosError
{
    public const string ValidacionFallida = "VALIDACION_FALLIDA";

    public const string EquipoNoEncontrado = "EQUIPO_NO_ENCONTRADO";
    public const string EquipoNombreDuplicado = "EQUIPO_NOMBRE_DUPLICADO";
    public const string EquipoConDependenciasConflicto = "EQUIPO_CON_DEPENDENCIAS_CONFLICTO";

    public const string JugadorNoEncontrado = "JUGADOR_NO_ENCONTRADO";
    public const string JugadorNoPerteneceAEquipo = "JUGADOR_NO_PERTENECE_A_EQUIPO";

    public const string PartidoNoEncontrado = "PARTIDO_NO_ENCONTRADO";
    public const string PartidoEquiposIgualesConflicto = "PARTIDO_EQUIPOS_IGUALES_CONFLICTO";
    public const string PartidoYaJugadoConflicto = "PARTIDO_YA_JUGADO_CONFLICTO";
    public const string PartidoGolesInconsistentes = "PARTIDO_GOLES_INCONSISTENTES";
}

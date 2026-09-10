using FluentAssertions;
using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.UnitTests.Application.Posiciones;

[TestClass]
public class CalculoTablaPosicionesTests
{
    [TestMethod]
    public void Ordenar_MismosPuntos_DesempataPorDiferenciaDeGol()
    {
        var estadisticas = new[]
        {
            CrearEstadisticas("Equipo A", victorias: 1, empates: 0, golesFavor: 3, golesContra: 2),
            CrearEstadisticas("Equipo B", victorias: 1, empates: 0, golesFavor: 5, golesContra: 1)
        };

        var tabla = CalculadorTablaPosiciones.Ordenar(estadisticas);

        tabla[0].NombreEquipo.Should().Be("Equipo B");
        tabla[0].Posicion.Should().Be(1);
        tabla[1].NombreEquipo.Should().Be("Equipo A");
        tabla[1].Posicion.Should().Be(2);
    }

    [TestMethod]
    public void Ordenar_MismosPuntosYMismaDiferencia_DesempataPorGolesAFavor()
    {
        var estadisticas = new[]
        {
            CrearEstadisticas("Equipo A", victorias: 1, empates: 0, golesFavor: 2, golesContra: 1),
            CrearEstadisticas("Equipo B", victorias: 1, empates: 0, golesFavor: 4, golesContra: 3)
        };

        var tabla = CalculadorTablaPosiciones.Ordenar(estadisticas);

        tabla[0].NombreEquipo.Should().Be("Equipo B");
        tabla[1].NombreEquipo.Should().Be("Equipo A");
    }

    [TestMethod]
    public void Ordenar_EquipoSinPartidosJugados_QuedaConCeroPuntosYCeroDiferencia()
    {
        var estadisticas = new[]
        {
            CrearEstadisticas("Equipo Sin Partidos", victorias: 0, empates: 0, golesFavor: 0, golesContra: 0)
        };

        var tabla = CalculadorTablaPosiciones.Ordenar(estadisticas);

        tabla[0].Puntos.Should().Be(0);
        tabla[0].DiferenciaGol.Should().Be(0);
    }

    private static EstadisticasEquipo CrearEstadisticas(string nombre, int victorias, int empates, int golesFavor, int golesContra)
    {
        return new EstadisticasEquipo
        {
            EquipoId = Guid.NewGuid(),
            NombreEquipo = nombre,
            PartidosJugados = victorias + empates,
            Victorias = victorias,
            Empates = empates,
            Derrotas = 0,
            GolesFavor = golesFavor,
            GolesContra = golesContra
        };
    }
}

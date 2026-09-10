using FluentAssertions;
using Mundialito.Application.Partidos.Consultas;

namespace Mundialito.UnitTests.Application.Posiciones;

[TestClass]
public class CalculadoraPuntosTests
{
    [TestMethod]
    public void Calcular_UnaVictoria_DevuelveTresPuntos()
    {
        var puntos = CalculadoraPuntos.Calcular(victorias: 1, empates: 0);

        puntos.Should().Be(3);
    }

    [TestMethod]
    public void Calcular_UnEmpate_DevuelveUnPunto()
    {
        var puntos = CalculadoraPuntos.Calcular(victorias: 0, empates: 1);

        puntos.Should().Be(1);
    }

    [TestMethod]
    public void Calcular_UnaDerrota_DevuelveCeroPuntos()
    {
        var puntos = CalculadoraPuntos.Calcular(victorias: 0, empates: 0);

        puntos.Should().Be(0);
    }

    [TestMethod]
    public void Calcular_VariasVictoriasYEmpates_SumaCorrectamente()
    {
        var puntos = CalculadoraPuntos.Calcular(victorias: 3, empates: 2);

        puntos.Should().Be(11);
    }
}

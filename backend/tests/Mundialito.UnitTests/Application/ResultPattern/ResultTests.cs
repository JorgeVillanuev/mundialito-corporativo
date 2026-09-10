using FluentAssertions;
using Mundialito.Application.Comun;

namespace Mundialito.UnitTests.Application.ResultPattern;

[TestClass]
public class ResultTests
{
    [TestMethod]
    public void Exito_CreaResultadoExitoso_SinCodigoDeError()
    {
        var resultado = Result.Exito();

        resultado.EsExitoso.Should().BeTrue();
        resultado.EsFallido.Should().BeFalse();
        resultado.CodigoError.Should().BeNull();
        resultado.MensajeError.Should().BeNull();
    }

    [TestMethod]
    public void Falla_CreaResultadoFallido_ConCodigoYMensaje()
    {
        var resultado = Result.Falla("CODIGO_ERROR", "Mensaje de error");

        resultado.EsExitoso.Should().BeFalse();
        resultado.EsFallido.Should().BeTrue();
        resultado.CodigoError.Should().Be("CODIGO_ERROR");
        resultado.MensajeError.Should().Be("Mensaje de error");
    }

    [TestMethod]
    public void ResultDeT_Exito_ExponeElValor()
    {
        var resultado = Result<int>.Exito(42);

        resultado.EsExitoso.Should().BeTrue();
        resultado.Valor.Should().Be(42);
    }

    [TestMethod]
    public void ResultDeT_Falla_NoExponeValorYExponeError()
    {
        var resultado = Result<int>.Falla("CODIGO_ERROR", "Mensaje de error");

        resultado.EsExitoso.Should().BeFalse();
        resultado.Valor.Should().Be(default);
        resultado.CodigoError.Should().Be("CODIGO_ERROR");
    }
}

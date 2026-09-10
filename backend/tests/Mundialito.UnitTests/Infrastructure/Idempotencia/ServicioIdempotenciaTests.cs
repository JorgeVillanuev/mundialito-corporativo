using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using Moq;
using Mundialito.Infrastructure.Idempotencia;

namespace Mundialito.UnitTests.Infrastructure.Idempotencia;

[TestClass]
public class ServicioIdempotenciaTests
{
    private const string Clave = "clave-1";
    private const string Ruta = "/api/v1/equipos";

    private Mock<IIdempotenciaStore> _storeMock = null!;
    private ServicioIdempotencia _servicio = null!;

    [TestInitialize]
    public void Inicializar()
    {
        _storeMock = new Mock<IIdempotenciaStore>();
        _servicio = new ServicioIdempotencia(_storeMock.Object);
    }

    [TestMethod]
    public async Task EvaluarAsync_ClaveNueva_DevuelveNuevo()
    {
        _storeMock
            .Setup(s => s.BuscarAsync(Clave, Ruta, It.IsAny<CancellationToken>()))
            .ReturnsAsync((RegistroIdempotencia?)null);

        var resultado = await _servicio.EvaluarAsync(Clave, Ruta, "{}", CancellationToken.None);

        resultado.Tipo.Should().Be(TipoResultadoIdempotencia.Nuevo);
    }

    [TestMethod]
    public async Task EvaluarAsync_ClaveExistenteConMismoPayload_DevuelveRespuestaOriginalSinReejecutar()
    {
        var payload = "{\"nombre\":\"Tigres\"}";

        _storeMock
            .Setup(s => s.BuscarAsync(Clave, Ruta, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RegistroIdempotencia
            {
                Id = Guid.NewGuid(),
                Clave = Clave,
                Ruta = Ruta,
                HashPayload = CalcularHash(payload),
                CodigoEstadoRespuesta = 201,
                CuerpoRespuesta = "{\"id\":\"abc\"}",
                CreadoEn = DateTime.UtcNow
            });

        var resultado = await _servicio.EvaluarAsync(Clave, Ruta, payload, CancellationToken.None);

        resultado.Tipo.Should().Be(TipoResultadoIdempotencia.RespuestaOriginal);
        resultado.CodigoEstadoRespuestaOriginal.Should().Be(201);
        resultado.CuerpoRespuestaOriginal.Should().Be("{\"id\":\"abc\"}");

        _storeMock.Verify(s => s.GuardarAsync(It.IsAny<RegistroIdempotencia>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestMethod]
    public async Task EvaluarAsync_ClaveExistenteConPayloadDistinto_DevuelveConflicto()
    {
        _storeMock
            .Setup(s => s.BuscarAsync(Clave, Ruta, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new RegistroIdempotencia
            {
                Id = Guid.NewGuid(),
                Clave = Clave,
                Ruta = Ruta,
                HashPayload = CalcularHash("{\"nombre\":\"Tigres\"}"),
                CodigoEstadoRespuesta = 201,
                CuerpoRespuesta = "{\"id\":\"abc\"}",
                CreadoEn = DateTime.UtcNow
            });

        var resultado = await _servicio.EvaluarAsync(Clave, Ruta, "{\"nombre\":\"Otro equipo\"}", CancellationToken.None);

        resultado.Tipo.Should().Be(TipoResultadoIdempotencia.Conflicto);
    }

    private static string CalcularHash(string payload) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(payload)));
}

using FluentAssertions;
using FluentValidation;
using Moq;
using Mundialito.Application.Comun;
using Mundialito.Application.Partidos.Comandos;
using Mundialito.Domain.Entidades;
using Mundialito.Domain.Enums;

namespace Mundialito.UnitTests.Application.Comandos;

[TestClass]
public class RegistrarResultadoPartidoHandlerTests
{
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private Mock<IPartidoRepository> _partidoRepositoryMock = null!;
    private Mock<IJugadorRepository> _jugadorRepositoryMock = null!;
    private IValidator<RegistrarResultadoPartidoCommand> _validador = null!;

    [TestInitialize]
    public void Inicializar()
    {
        _partidoRepositoryMock = new Mock<IPartidoRepository>();
        _jugadorRepositoryMock = new Mock<IJugadorRepository>();

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.SetupGet(u => u.Partidos).Returns(_partidoRepositoryMock.Object);
        _unitOfWorkMock.SetupGet(u => u.Jugadores).Returns(_jugadorRepositoryMock.Object);

        _validador = new RegistrarResultadoPartidoCommandValidator();
    }

    [TestMethod]
    public async Task Manejar_PartidoYaJugado_DevuelveConflicto()
    {
        var partido = Partido.Crear(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow);
        partido.RegistrarResultado(1, 0);

        _partidoRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(partido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(partido);

        var handler = new RegistrarResultadoPartidoCommandHandler(_unitOfWorkMock.Object, _validador);
        var comando = new RegistrarResultadoPartidoCommand(partido.Id, 1, 0, Array.Empty<GolJugadorEntrada>());

        var resultado = await handler.Manejar(comando, CancellationToken.None);

        resultado.EsExitoso.Should().BeFalse();
        resultado.CodigoError.Should().Be(CodigosError.PartidoYaJugadoConflicto);
    }

    [TestMethod]
    public async Task Manejar_SumaDeGoleadoresNoCoincideConMarcador_DevuelveGolesInconsistentes()
    {
        var equipoLocalId = Guid.NewGuid();
        var equipoVisitanteId = Guid.NewGuid();
        var partido = Partido.Crear(equipoLocalId, equipoVisitanteId, DateTime.UtcNow);
        var jugadorLocal = Jugador.Crear(equipoLocalId, "Jugador Local", PosicionJugador.Delantero);

        _partidoRepositoryMock
            .Setup(r => r.ObtenerPorIdAsync(partido.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(partido);

        _jugadorRepositoryMock
            .Setup(r => r.ObtenerPorIdsAsync(It.IsAny<IReadOnlyCollection<Guid>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Jugador> { jugadorLocal });

        var handler = new RegistrarResultadoPartidoCommandHandler(_unitOfWorkMock.Object, _validador);
        var goleadores = new[] { new GolJugadorEntrada(jugadorLocal.Id, 1) };
        var comando = new RegistrarResultadoPartidoCommand(partido.Id, 2, 0, goleadores);

        var resultado = await handler.Manejar(comando, CancellationToken.None);

        resultado.EsExitoso.Should().BeFalse();
        resultado.CodigoError.Should().Be(CodigosError.PartidoGolesInconsistentes);
    }
}

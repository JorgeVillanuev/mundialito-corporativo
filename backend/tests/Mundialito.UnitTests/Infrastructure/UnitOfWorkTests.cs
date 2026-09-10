using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mundialito.Application.Comun;
using Mundialito.Domain.Comun;
using Mundialito.Domain.Entidades;
using Mundialito.Infrastructure.EventosDominio;
using Mundialito.Infrastructure.Persistencia;

namespace Mundialito.UnitTests.Infrastructure;

[TestClass]
public class UnitOfWorkTests
{
    private MundialitoDbContext _dbContext = null!;
    private Mock<IDomainEventDispatcher> _dispatcherMock = null!;
    private UnitOfWork _unitOfWork = null!;

    [TestInitialize]
    public void Inicializar()
    {
        var opciones = new DbContextOptionsBuilder<MundialitoDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dbContext = new MundialitoDbContext(opciones);
        _dispatcherMock = new Mock<IDomainEventDispatcher>();

        _unitOfWork = new UnitOfWork(
            _dbContext,
            _dispatcherMock.Object,
            Mock.Of<IEquipoRepository>(),
            Mock.Of<IJugadorRepository>(),
            Mock.Of<IPartidoRepository>());
    }

    [TestCleanup]
    public void Limpiar()
    {
        _dbContext.Dispose();
    }

    [TestMethod]
    public async Task GuardarCambiosAsync_ConEntidadNueva_PersisteLosCambios()
    {
        var equipo = Equipo.Crear("Tigres FC", "San Salvador");
        _dbContext.Equipos.Add(equipo);

        await _unitOfWork.GuardarCambiosAsync(CancellationToken.None);

        (await _dbContext.Equipos.AnyAsync(e => e.Id == equipo.Id)).Should().BeTrue();
    }

    [TestMethod]
    public async Task GuardarCambiosAsync_DespachaEventosSoloTrasCommitExitoso()
    {
        var equipo = Equipo.Crear("Tigres FC", "San Salvador");
        _dbContext.Equipos.Add(equipo);

        await _unitOfWork.GuardarCambiosAsync(CancellationToken.None);

        _dispatcherMock.Verify(
            d => d.DespacharAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestMethod]
    public async Task GuardarCambiosAsync_LimpiaLosEventosDeLaEntidadTrasDespachar()
    {
        var equipo = Equipo.Crear("Tigres FC", "San Salvador");
        _dbContext.Equipos.Add(equipo);

        await _unitOfWork.GuardarCambiosAsync(CancellationToken.None);

        equipo.EventosDominio.Should().BeEmpty();
    }

    [TestMethod]
    public async Task GuardarCambiosAsync_SinEntidadesConEventosPendientes_NoDespachaNada()
    {
        var equipo = Equipo.Crear("Tigres FC", "San Salvador");
        equipo.LimpiarEventos();
        _dbContext.Equipos.Add(equipo);

        await _unitOfWork.GuardarCambiosAsync(CancellationToken.None);

        _dispatcherMock.Verify(
            d => d.DespacharAsync(It.IsAny<IDomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}

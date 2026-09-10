using Microsoft.EntityFrameworkCore;
using Mundialito.Domain.Entidades;
using Mundialito.Infrastructure.Idempotencia;

namespace Mundialito.Infrastructure.Persistencia;

public class MundialitoDbContext : DbContext
{
    public MundialitoDbContext(DbContextOptions<MundialitoDbContext> options) : base(options)
    {
    }

    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Jugador> Jugadores => Set<Jugador>();
    public DbSet<Partido> Partidos => Set<Partido>();
    public DbSet<GolPartido> GolesPartido => Set<GolPartido>();

    public DbSet<RegistroIdempotencia> RegistrosIdempotencia => Set<RegistroIdempotencia>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MundialitoDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

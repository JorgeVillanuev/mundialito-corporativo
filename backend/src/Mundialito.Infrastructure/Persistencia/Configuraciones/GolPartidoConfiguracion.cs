using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mundialito.Domain.Entidades;

namespace Mundialito.Infrastructure.Persistencia.Configuraciones;

public class GolPartidoConfiguracion : IEntityTypeConfiguration<GolPartido>
{
    public void Configure(EntityTypeBuilder<GolPartido> builder)
    {
        builder.ToTable("GolesPartido");
        builder.HasKey(g => g.Id);

        builder.HasOne<Partido>()
            .WithMany()
            .HasForeignKey(g => g.PartidoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Jugador>()
            .WithMany()
            .HasForeignKey(g => g.JugadorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(g => new { g.PartidoId, g.JugadorId })
            .IsUnique()
            .HasDatabaseName("UQ_GolesPartido_Partido_Jugador");
    }
}

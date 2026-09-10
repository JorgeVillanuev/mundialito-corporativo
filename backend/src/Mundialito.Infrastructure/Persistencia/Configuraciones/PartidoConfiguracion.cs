using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mundialito.Domain.Entidades;

namespace Mundialito.Infrastructure.Persistencia.Configuraciones;

public class PartidoConfiguracion : IEntityTypeConfiguration<Partido>
{
    public void Configure(EntityTypeBuilder<Partido> builder)
    {
        builder.ToTable("Partidos", t => t.HasCheckConstraint(
            "CK_Partidos_EquiposDistintos", "[EquipoLocalId] <> [EquipoVisitanteId]"));

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Estado)
            .IsConcurrencyToken();

        builder.HasOne<Equipo>()
            .WithMany()
            .HasForeignKey(p => p.EquipoLocalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Equipo>()
            .WithMany()
            .HasForeignKey(p => p.EquipoVisitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.Estado, p.FechaHora })
            .HasDatabaseName("IX_Partidos_Estado_FechaHora");
    }
}

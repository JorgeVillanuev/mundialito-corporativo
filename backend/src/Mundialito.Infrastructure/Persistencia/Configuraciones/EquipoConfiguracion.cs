using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mundialito.Domain.Entidades;

namespace Mundialito.Infrastructure.Persistencia.Configuraciones;

public class EquipoConfiguracion : IEntityTypeConfiguration<Equipo>
{
    public void Configure(EntityTypeBuilder<Equipo> builder)
    {
        builder.ToTable("Equipos");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nombre)
            .HasMaxLength(Equipo.LongitudMaximaNombre)
            .IsRequired();

        builder.Property(e => e.CiudadOrigen)
            .HasMaxLength(150)
            .IsRequired();

        builder.HasIndex(e => e.Nombre)
            .IsUnique()
            .HasDatabaseName("UQ_Equipos_Nombre");
    }
}

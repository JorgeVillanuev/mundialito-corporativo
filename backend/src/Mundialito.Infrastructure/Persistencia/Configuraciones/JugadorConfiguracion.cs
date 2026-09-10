using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mundialito.Domain.Entidades;

namespace Mundialito.Infrastructure.Persistencia.Configuraciones;

public class JugadorConfiguracion : IEntityTypeConfiguration<Jugador>
{
    public void Configure(EntityTypeBuilder<Jugador> builder)
    {
        builder.ToTable("Jugadores");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Nombre)
            .HasMaxLength(Jugador.LongitudMaximaNombre)
            .IsRequired();

        builder.HasOne<Equipo>()
            .WithMany()
            .HasForeignKey(j => j.EquipoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(j => j.EquipoId);
    }
}

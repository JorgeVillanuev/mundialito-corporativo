using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mundialito.Infrastructure.Idempotencia;

namespace Mundialito.Infrastructure.Persistencia.Configuraciones;

public class RegistroIdempotenciaConfiguracion : IEntityTypeConfiguration<RegistroIdempotencia>
{
    public void Configure(EntityTypeBuilder<RegistroIdempotencia> builder)
    {
        builder.ToTable("RegistrosIdempotencia");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Clave).HasMaxLength(200).IsRequired();

        builder.Property(r => r.Ruta).HasMaxLength(300).IsRequired();

        builder.Property(r => r.HashPayload).HasMaxLength(64).IsRequired();

        builder.Property(r => r.CuerpoRespuesta).IsRequired();

        builder.HasIndex(r => new { r.Clave, r.Ruta })
            .IsUnique()
            .HasDatabaseName("UQ_RegistrosIdempotencia_Clave_Ruta");
    }
}

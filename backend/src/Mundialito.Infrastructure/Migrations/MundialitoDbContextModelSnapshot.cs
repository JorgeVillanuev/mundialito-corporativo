using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mundialito.Infrastructure.Persistencia;

#nullable disable

namespace Mundialito.Infrastructure.Migrations
{
    [DbContext(typeof(MundialitoDbContext))]
    partial class MundialitoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Mundialito.Domain.Entidades.Equipo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CiudadOrigen")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<DateTime>("CreadoEn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("Nombre")
                        .IsUnique()
                        .HasDatabaseName("UQ_Equipos_Nombre");

                    b.ToTable("Equipos", (string)null);
                });

            modelBuilder.Entity("Mundialito.Domain.Entidades.GolPartido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CantidadGoles")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreadoEn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("JugadorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PartidoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("JugadorId");

                    b.HasIndex("PartidoId", "JugadorId")
                        .IsUnique()
                        .HasDatabaseName("UQ_GolesPartido_Partido_Jugador");

                    b.ToTable("GolesPartido", (string)null);
                });

            modelBuilder.Entity("Mundialito.Domain.Entidades.Jugador", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreadoEn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EquipoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Posicion")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EquipoId");

                    b.ToTable("Jugadores", (string)null);
                });

            modelBuilder.Entity("Mundialito.Domain.Entidades.Partido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreadoEn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EquipoLocalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EquipoVisitanteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Estado")
                        .IsConcurrencyToken()
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaHora")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("FechaResultadoRegistrado")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GolesLocal")
                        .HasColumnType("int");

                    b.Property<int?>("GolesVisitante")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EquipoLocalId");

                    b.HasIndex("EquipoVisitanteId");

                    b.HasIndex("Estado", "FechaHora")
                        .HasDatabaseName("IX_Partidos_Estado_FechaHora");

                    b.ToTable("Partidos", null, t =>
                        {
                            t.HasCheckConstraint("CK_Partidos_EquiposDistintos", "[EquipoLocalId] <> [EquipoVisitanteId]");
                        });
                });

            modelBuilder.Entity("Mundialito.Infrastructure.Idempotencia.RegistroIdempotencia", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("CodigoEstadoRespuesta")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreadoEn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CuerpoRespuesta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HashPayload")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Ruta")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("Clave", "Ruta")
                        .IsUnique()
                        .HasDatabaseName("UQ_RegistrosIdempotencia_Clave_Ruta");

                    b.ToTable("RegistrosIdempotencia", (string)null);
                });

            modelBuilder.Entity("Mundialito.Domain.Entidades.GolPartido", b =>
                {
                    b.HasOne("Mundialito.Domain.Entidades.Jugador", null)
                        .WithMany()
                        .HasForeignKey("JugadorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mundialito.Domain.Entidades.Partido", null)
                        .WithMany()
                        .HasForeignKey("PartidoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mundialito.Domain.Entidades.Jugador", b =>
                {
                    b.HasOne("Mundialito.Domain.Entidades.Equipo", null)
                        .WithMany()
                        .HasForeignKey("EquipoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Mundialito.Domain.Entidades.Partido", b =>
                {
                    b.HasOne("Mundialito.Domain.Entidades.Equipo", null)
                        .WithMany()
                        .HasForeignKey("EquipoLocalId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mundialito.Domain.Entidades.Equipo", null)
                        .WithMany()
                        .HasForeignKey("EquipoVisitanteId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

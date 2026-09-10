using Microsoft.EntityFrameworkCore;
using Mundialito.Domain.Entidades;
using Mundialito.Domain.Enums;
using Mundialito.Infrastructure.Persistencia;

namespace Mundialito.Infrastructure.Seed;

public static class DataSeeder
{
    public static async Task SembrarSiVacioAsync(MundialitoDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Equipos.AnyAsync(cancellationToken))
            return;

        var equipos = new[]
        {
            Equipo.Crear("Tigres FC", "San Salvador"),
            Equipo.Crear("Aguilas Doradas", "Santa Ana"),
            Equipo.Crear("Leones del Valle", "San Miguel"),
            Equipo.Crear("Halcones Unidos", "La Libertad")
        };
        dbContext.Equipos.AddRange(equipos);

        var posiciones = new[]
        {
            PosicionJugador.Arquero,
            PosicionJugador.Defensor,
            PosicionJugador.Defensor,
            PosicionJugador.Mediocampista,
            PosicionJugador.Delantero
        };

        var jugadoresPorEquipo = new Dictionary<Guid, List<Jugador>>();

        foreach (var equipo in equipos)
        {
            var jugadores = new List<Jugador>();
            for (var i = 0; i < posiciones.Length; i++)
                jugadores.Add(Jugador.Crear(equipo.Id, $"{equipo.Nombre} - Jugador {i + 1}", posiciones[i]));

            jugadoresPorEquipo[equipo.Id] = jugadores;
            dbContext.Jugadores.AddRange(jugadores);
        }

        var fechaBase = DateTime.UtcNow.Date.AddDays(-14);

        var partidos = new[]
        {
            Partido.Crear(equipos[0].Id, equipos[1].Id, fechaBase),
            Partido.Crear(equipos[2].Id, equipos[3].Id, fechaBase.AddDays(1)),
            Partido.Crear(equipos[0].Id, equipos[2].Id, fechaBase.AddDays(7)),
            Partido.Crear(equipos[1].Id, equipos[3].Id, fechaBase.AddDays(7)),
            Partido.Crear(equipos[0].Id, equipos[3].Id, fechaBase.AddDays(14)),
            Partido.Crear(equipos[1].Id, equipos[2].Id, fechaBase.AddDays(14))
        };

        partidos[0].RegistrarResultado(2, 1);
        partidos[1].RegistrarResultado(0, 0);
        partidos[2].RegistrarResultado(3, 2);

        dbContext.Partidos.AddRange(partidos);

        var delanteroEquipo0 = jugadoresPorEquipo[equipos[0].Id][4];
        var mediocampistaEquipo0 = jugadoresPorEquipo[equipos[0].Id][3];
        var delanteroEquipo1 = jugadoresPorEquipo[equipos[1].Id][4];
        var delanteroEquipo2 = jugadoresPorEquipo[equipos[2].Id][4];

        var goles = new List<GolPartido>
        {
            GolPartido.Crear(partidos[0].Id, delanteroEquipo0.Id, 2),
            GolPartido.Crear(partidos[0].Id, delanteroEquipo1.Id, 1),

            GolPartido.Crear(partidos[2].Id, delanteroEquipo0.Id, 2),
            GolPartido.Crear(partidos[2].Id, mediocampistaEquipo0.Id, 1),
            GolPartido.Crear(partidos[2].Id, delanteroEquipo2.Id, 2)
        };
        dbContext.GolesPartido.AddRange(goles);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

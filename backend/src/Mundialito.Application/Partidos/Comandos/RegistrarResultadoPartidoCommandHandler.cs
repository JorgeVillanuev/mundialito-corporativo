using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mundialito.Application.Comun;
using Mundialito.Domain.Entidades;
using Mundialito.Domain.Enums;

namespace Mundialito.Application.Partidos.Comandos;

public sealed class RegistrarResultadoPartidoCommandHandler : ICommandHandler<RegistrarResultadoPartidoCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegistrarResultadoPartidoCommand> _validador;

    public RegistrarResultadoPartidoCommandHandler(IUnitOfWork unitOfWork, IValidator<RegistrarResultadoPartidoCommand> validador)
    {
        _unitOfWork = unitOfWork;
        _validador = validador;
    }

    public async Task<Result> Manejar(RegistrarResultadoPartidoCommand comando, CancellationToken cancellationToken)
    {
        var errorValidacion = await Validaciones.ValidarAsync(_validador, comando, cancellationToken);
        if (errorValidacion is not null)
            return Result.Falla(CodigosError.ValidacionFallida, errorValidacion);

        var partido = await _unitOfWork.Partidos.ObtenerPorIdAsync(comando.PartidoId, cancellationToken);
        if (partido is null)
            return Result.Falla(CodigosError.PartidoNoEncontrado, $"No existe un partido con id '{comando.PartidoId}'.");

        if (partido.Estado == EstadoPartido.Jugado)
            return Result.Falla(CodigosError.PartidoYaJugadoConflicto, "El partido ya tiene un resultado registrado.");

        var idsJugadores = comando.Goleadores.Select(g => g.JugadorId).Distinct().ToList();
        var jugadores = await _unitOfWork.Jugadores.ObtenerPorIdsAsync(idsJugadores, cancellationToken);
        if (jugadores.Count != idsJugadores.Count)
            return Result.Falla(CodigosError.JugadorNoEncontrado, "Uno o más jugadores goleadores no existen.");

        var jugadoresPorId = jugadores.ToDictionary(j => j.Id);

        var todosPertenecenAEquiposDelPartido = comando.Goleadores.All(g =>
        {
            var equipoIdJugador = jugadoresPorId[g.JugadorId].EquipoId;
            return equipoIdJugador == partido.EquipoLocalId || equipoIdJugador == partido.EquipoVisitanteId;
        });
        if (!todosPertenecenAEquiposDelPartido)
            return Result.Falla(CodigosError.JugadorNoPerteneceAEquipo, "Un jugador goleador no pertenece a ninguno de los dos equipos del partido.");

        var golesLocalGoleadores = comando.Goleadores
            .Where(g => jugadoresPorId[g.JugadorId].EquipoId == partido.EquipoLocalId)
            .Sum(g => g.CantidadGoles);
        var golesVisitanteGoleadores = comando.Goleadores
            .Where(g => jugadoresPorId[g.JugadorId].EquipoId == partido.EquipoVisitanteId)
            .Sum(g => g.CantidadGoles);

        if (golesLocalGoleadores != comando.GolesLocal || golesVisitanteGoleadores != comando.GolesVisitante)
            return Result.Falla(CodigosError.PartidoGolesInconsistentes, "La suma de goles de los goleadores no coincide con el marcador informado.");

        partido.RegistrarResultado(comando.GolesLocal, comando.GolesVisitante);

        var goles = comando.Goleadores
            .Select(g => GolPartido.Crear(partido.Id, g.JugadorId, g.CantidadGoles))
            .ToList();
        _unitOfWork.Partidos.AgregarGoles(goles);

        try
        {
            await _unitOfWork.GuardarCambiosAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Result.Falla(CodigosError.PartidoYaJugadoConflicto, "El partido ya tiene un resultado registrado.");
        }

        return Result.Exito();
    }
}

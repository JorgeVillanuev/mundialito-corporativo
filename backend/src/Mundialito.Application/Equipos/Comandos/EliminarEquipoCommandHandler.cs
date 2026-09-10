using Mundialito.Application.Comun;

namespace Mundialito.Application.Equipos.Comandos;

public sealed class EliminarEquipoCommandHandler : ICommandHandler<EliminarEquipoCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public EliminarEquipoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Manejar(EliminarEquipoCommand comando, CancellationToken cancellationToken)
    {
        var equipo = await _unitOfWork.Equipos.ObtenerPorIdAsync(comando.Id, cancellationToken);
        if (equipo is null)
            return Result.Exito();

        var tieneDependencias = await _unitOfWork.Equipos.TieneDependenciasAsync(comando.Id, cancellationToken);
        if (tieneDependencias)
            return Result.Falla(CodigosError.EquipoConDependenciasConflicto, "El equipo tiene jugadores o partidos asociados y no puede eliminarse.");

        _unitOfWork.Equipos.Eliminar(equipo);
        await _unitOfWork.GuardarCambiosAsync(cancellationToken);

        return Result.Exito();
    }
}

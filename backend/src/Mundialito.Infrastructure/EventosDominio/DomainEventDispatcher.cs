using Microsoft.Extensions.Logging;
using Mundialito.Domain.Comun;

namespace Mundialito.Infrastructure.EventosDominio;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(ILogger<DomainEventDispatcher> logger)
    {
        _logger = logger;
    }

    public Task DespacharAsync(IDomainEvent evento, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Evento de dominio despachado: {TipoEvento} {@Evento}", evento.GetType().Name, evento);

        return Task.CompletedTask;
    }
}

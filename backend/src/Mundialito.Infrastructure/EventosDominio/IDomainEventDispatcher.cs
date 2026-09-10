using Mundialito.Domain.Comun;

namespace Mundialito.Infrastructure.EventosDominio;

public interface IDomainEventDispatcher
{
    Task DespacharAsync(IDomainEvent evento, CancellationToken cancellationToken);
}

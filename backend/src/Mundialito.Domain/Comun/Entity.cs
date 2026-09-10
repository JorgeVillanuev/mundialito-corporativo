namespace Mundialito.Domain.Comun;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    public DateTime CreadoEn { get; protected set; }

    private readonly List<IDomainEvent> _eventosDominio = new();

    public IReadOnlyCollection<IDomainEvent> EventosDominio => _eventosDominio.AsReadOnly();

    protected Entity()
    {
        Id = Guid.NewGuid();
        CreadoEn = DateTime.UtcNow;
    }

    protected void AgregarEvento(IDomainEvent evento)
    {
        _eventosDominio.Add(evento);
    }

    public void LimpiarEventos()
    {
        _eventosDominio.Clear();
    }
}

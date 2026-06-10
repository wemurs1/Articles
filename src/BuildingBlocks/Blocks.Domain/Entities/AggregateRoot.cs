namespace Blocks.Domain.Entities;

public interface IAggregateRoot : IAggregateRoot<int>;

public abstract class AggregateRoot : AggregateRoot<int>, IAggregateRoot, IAuditedEntity;

public interface IAggregateRoot<TPrimaryKey> : IAuditedEntity<TPrimaryKey> where TPrimaryKey : struct
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }
    public void AddDomainEvent(IDomainEvent eventItem);
    public void ClearDomainEvents();
}

public abstract class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey> where TPrimaryKey : struct
{
    public TPrimaryKey CreatedById { get; init; }
    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    public TPrimaryKey? LastModifiedById { get; set; }
    public DateTime? LastModifiedOn { get; set; }


    private List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
    public void AddDomainEvent(IDomainEvent eventItem) => _domainEvents.Add(eventItem);
    public void ClearDomainEvents() => _domainEvents.Clear();
}

namespace Blocks.Domain;

public interface IAuditableAction
{
    DateTime CreatedOn => DateTime.UtcNow;
    int CreatedById { get; set; }

}

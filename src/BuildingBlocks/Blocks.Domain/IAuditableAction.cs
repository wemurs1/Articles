namespace Blocks.Domain;

public interface IAuditableAction
{
    DateTime CreatedOn => DateTime.UtcNow;
    int CreatedById { get; set; }
    public string Action { get; }
}

public interface IAuditableAction<TActionType> : IAuditableAction where TActionType : Enum
{
    TActionType ActionType { get; }
    string IAuditableAction.Action => ActionType.ToString();
}
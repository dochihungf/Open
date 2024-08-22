namespace Open.Core.SeedWork.Interfaces;

public interface ICoreEntity
{
    string GetTableName();

    object? this[string propertyName] { get; set; }
}

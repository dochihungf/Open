namespace Open.Core.SeedWork.Interfaces;

public interface IDateTracking
{
    DateTime CreatedDate { get; set; }
    
    DateTime? LastModifiedDate { get; set; }
}

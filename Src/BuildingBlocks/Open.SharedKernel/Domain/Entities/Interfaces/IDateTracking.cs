namespace Open.SharedKernel.Domain.Entities.Interfaces;

public interface IDateTracking
{
    DateTime CreatedDate { get; set; }
    DateTime? LastModifiedDate { get; set; }
}
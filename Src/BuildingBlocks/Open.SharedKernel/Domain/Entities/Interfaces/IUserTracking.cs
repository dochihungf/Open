namespace Open.SharedKernel.Domain.Entities.Interfaces;

public interface IUserTracking
{
    Guid CreatedBy { get; set; }
    Guid? LastModifiedBy { get; set; }
}
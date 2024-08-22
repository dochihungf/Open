namespace Open.Core.SeedWork.Interfaces;

public interface IUserTracking
{
    Guid CreatedBy { get; set; }

    Guid? LastModifiedBy { get; set; }
}

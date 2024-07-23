namespace Open.SharedKernel.Domain.Entities.Interfaces;

public interface IAuditable : IDateTracking, IUserTracking, ISoftDelete
{
    
}
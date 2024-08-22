namespace Open.Core.SeedWork.Interfaces;

public interface ISoftDelete
{
    DateTime? DeletedDate { get; set; }
    
    Guid? DeletedBy { get; set; }
    
    bool IsDeleted { get; set; }
}

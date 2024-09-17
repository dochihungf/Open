using Open.Core.SeedWork;
using Open.Core.SeedWork.Interfaces;
using Open.User.Domain.SeedWork;

namespace Open.User.Domain.UserAggregate;

public class UserAddress : EntityBase, IPersonalizeEntity
{
    public string Street { get; set; } // Địa chỉ đường phố
    
    public string City { get; set; } // Thành phố
    
    public string State { get; set; } // Tỉnh thành phố
    
    public string PostalCode { get; set; } // Mã bưu điện
    
    public string Country { get; set; } // Quốc gia
    
    public string Address { get; set; } // Địa chỉ
    
    public bool IsPrimary { get; set; } // Địa chỉ chính
    
    public Guid OwnerId { get; set; }
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
    
}

using Open.Core.SeedWork;

namespace Open.User.Domain.UserAggregate;

public class UserAddress : PersonalizedEntityAuditBase
{
    public string Street { get; set; } // Địa chỉ đường phố
    
    public string City { get; set; } // Thành phố
    
    public string State { get; set; } // Tỉnh thành phố
    
    public string PostalCode { get; set; } // Mã bưu điện
    
    public string Country { get; set; } // Quốc gia
    
    public string Address { get; set; } // Địa chỉ
    
    public bool IsPrimary { get; set; } // Địa chỉ chính
    
    #region Navigations
    
    public virtual User User { get; set; }
    
    #endregion
}

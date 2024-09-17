using Open.Core.SeedWork;
using Open.Core.SeedWork.Events;
using Open.Core.SeedWork.Interfaces;
using Open.User.Domain.Enums;
using Open.User.Domain.SeedWork;

namespace Open.User.Domain.UserAggregate;

public class User : EntityBase, IAggregateRoot
{
    public string Username { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string Salt { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public bool ConfirmedPhone { get; set; }
    
    public string Email { get; set; }
    
    public bool ConfirmedEmail { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string FullName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public GenderType Gender { get; set; }
    
    #region Navigations
    
    public virtual UserAvatar Avatar { get; set; }
    
    public virtual UserProfile Profile { get; set; }
    
    public virtual UserConfig Config { get; set; }
    
    public ICollection<UserAddress> Addresses { get; set; }
    
    #endregion
    
}

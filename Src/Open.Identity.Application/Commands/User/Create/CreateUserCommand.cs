using Open.Identity.Domain.Enums;
using Open.Security.Enums;

namespace Open.Identity.Application.Commands;

public sealed class CreateUserCommand
{
    public string Username { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string Salt { get; set; }
    
    public string PhoneNumber { get; set; }

    public bool ConfirmedPhone { get; set; } = true;
    
    public string Email { get; set; }

    public bool ConfirmedEmail { get; set; } = true;
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public GenderType Gender { get; set; } = GenderType.Other;
    
    public AuthenticatorType AuthenticatorType { get; set; } = AuthenticatorType.None;
}

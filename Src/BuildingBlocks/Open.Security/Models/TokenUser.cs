namespace Open.Security.Models;

public class TokenUser(
    Guid id,
    string username,
    string passwordHash,
    string salt,
    string phoneNumber,
    bool confirmedPhone,
    string email,
    bool confirmedEmail,
    string firstName,
    string lastName,
    string address,
    DateTime dateOfBirth,
    string permission)
{
    public Guid Id { get; set; } = id;

    public string Username { get; set; } = username;

    public string PasswordHash { get; set; } = passwordHash;

    public string Salt { get; set; } = salt;

    public string PhoneNumber { get; set; } = phoneNumber;

    public bool ConfirmedPhone { get; set; } = confirmedPhone;

    public string Email { get; set; } = email;

    public bool ConfirmedEmail { get; set; } = confirmedEmail;

    public string FirstName { get; set; } = firstName;

    public string LastName { get; set; } = lastName;

    public string Address { get; set; } = address;

    public DateTime DateOfBirth { get; set; } = dateOfBirth;

    public string Permission { get; set; } = permission;

    public List<string> RoleNames { get; set; } = new List<string>();
    
}
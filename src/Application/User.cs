using Abp.Domain.Entities;

namespace Application;

public class User : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public User() { }

    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }
}

namespace EmpAnalytics.Domain.Users;

public class User
{
    public User(Guid userId, string username, string lastname)
    {
        UserId = userId;
        Username = username;
        Lastname = lastname;
    }

    public Guid UserId { get; private set; }

    public string Username { get; private set; } = string.Empty;

    public string Lastname { get; private set; } = string.Empty;
}
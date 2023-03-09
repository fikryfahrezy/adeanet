namespace Adea.User;
public class RegisterUser
{
    public string Username;
    public string Password;
    public bool IsOfficer;

    public RegisterUser(string username, string password, bool isOfficer)
    {
        Username = username;
        Password = password;
        IsOfficer = isOfficer;
    }
}

public class LoginUser
{
    public string Username;
    public string Password;

    public LoginUser(string username, string password)
    {
        Username = username;
        Password = password;
    }
}

public class Member
{
    public string Id;
    public string Username;
    public string Password;
    public bool IsOfficer;

    public Member(string id, string username, string password, bool isOfficer)
    {
        Id = id;
        Username = username;
        Password = password;
        IsOfficer = isOfficer;
    }
}
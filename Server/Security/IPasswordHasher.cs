namespace Server.Security
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyMatch(string password, string passwordHash);
    }
}

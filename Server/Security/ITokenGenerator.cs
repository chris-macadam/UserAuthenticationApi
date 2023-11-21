namespace Server.Security
{
    public interface ITokenGenerator
    {
        string GenerateToken(int id, string username);

        string GenerateToken(int id, string username, string role);
    }
}

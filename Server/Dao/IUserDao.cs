using Server.Models;

namespace Server.Dao
{
    public interface IUserDao
    {
        User CreateUser(string username, string hashedPassword);
        User? GetUserByUsername(string username);
    }
}

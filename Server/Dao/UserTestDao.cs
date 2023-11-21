using Server.Models;

namespace Server.Dao
{
    public class UserTestDao : IUserDao
    {
        public List<User> Users { get; private set; }

        public UserTestDao()
        {
            Users = new List<User>();
        }

        public UserTestDao(List<User> users)
        {
            Users = users;
        }

        public User CreateUser(string username, string passwordHash)
        {
            User user = new User()
            {
                Id = Users.Count,
                Username = username,
                PasswordHash = passwordHash
            };

            Users.Add(user);
            return user;
        }

        public User? GetUserByUsername(string username)
        {
            foreach (var user in Users)
            {
                if (user.Username == username)
                {
                    return user;
                }
            }
            return null;
        }
    }
}

namespace Server.Models
{
    /// <summary>
    /// Data object sent to the server to request to login or register
    /// </summary>
    public class UserLogin
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

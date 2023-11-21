namespace Server.Models
{
    /// <summary>
    /// Data object that gets sent to the client upon sucessful login
    /// </summary>
    public class UserAuthentication
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}

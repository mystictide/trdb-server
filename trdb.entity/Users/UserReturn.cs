
namespace trdb.entity.Users
{
    public class UserReturn
    {
        public string Username { get; set; }
        public string? Token { get; set; }
        public UserSettings? Settings { get; set; }
        public UserFollows? Following { get; set; }
        public UserFollows? Followers { get; set; }
        public UserBlocklist? Blocked { get; set; }
    }
}

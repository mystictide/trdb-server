using trdb.entity.Users;

namespace trdb.entity.Returns
{
    public class UserReturn
    {
        public int? ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Token { get; set; }
        public UserSettings? Settings { get; set; }
        public UserFollows? Following { get; set; }
        public UserFollows? Followers { get; set; }
        public UserBlocklist? Blocked { get; set; }
    }
}

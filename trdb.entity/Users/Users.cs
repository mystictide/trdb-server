using Dapper.Contrib.Extensions;
using trdb.entity.Users.Settings;

namespace trdb.entity.Users
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int ID { get; set; }
        public string? Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AuthType { get; set; }
        public bool IsActive { get; set; }

        [Write(false)]
        public string? Token { get; set; }
        [Write(false)]
        public UserSettings? Settings { get; set; }
        [Write(false)]
        public List<Users>? Following { get; set; }
        [Write(false)]
        public List<Users>? Followers { get; set; }
        [Write(false)]
        public List<Users>? Blocklist{ get; set; }
    }
}

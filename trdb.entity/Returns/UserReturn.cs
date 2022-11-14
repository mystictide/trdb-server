using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using trdb.entity.Helpers;
using trdb.entity.Users;
using trdb.entity.Users.Settings;

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
        [Write(false)]
        [JsonProperty("pager")]
        public Page? Pager { get; set; }
    }
}

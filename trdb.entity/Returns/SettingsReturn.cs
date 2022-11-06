using Newtonsoft.Json;

namespace trdb.entity.Returns
{
    public class SettingsReturn
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("bio")]
        public string Bio { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
    }
}

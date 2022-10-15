using Newtonsoft.Json;

namespace trdb.api.Models
{
    public class TMDB_Error
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("status_code")]
        public int Code { get; set; }
        [JsonProperty("status_message")]
        public string? Message { get; set; }
    }
}

using Newtonsoft.Json;

namespace trdb.api.Models
{
    public class MovieReturn
    {
        [JsonProperty("id")]
        public int TMDB_ID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("backdrop_path")]
        public string Backdrop_URL { get; set; }
        [JsonProperty("poster_path")]
        public string Poster_URL { get; set; }
        [JsonProperty("release_date")]
        public string Release_Date { get; set; }
    }
}

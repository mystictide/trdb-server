using Newtonsoft.Json;

namespace trdb.entity.Returns
{
    public class WeeklyReturn
    {
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("id")]
        public int TMDB_ID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("backdrop_path")]
        public string Backdrop_URL { get; set; }
        [JsonProperty("poster_path")]
        public string Poster_URL { get; set; }
        [JsonProperty("expires")]
        public DateTime WeeklyExpiryDate { get; set; }
    }
}

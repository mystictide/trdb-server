using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.UserMovies
{
    [Table("UserWatchlistJunction")]
    public class UserWatchlist
    {
        [Key]
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonIgnore]
        public int UserID { get; set; }
        [JsonProperty("tmdb_id")]
        public int MovieID { get; set; }
        [JsonProperty("date_added")]
        public DateTime Date { get; set; }
    }
}
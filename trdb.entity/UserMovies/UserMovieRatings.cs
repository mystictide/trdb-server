using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.UserMovies
{
    [Table("UserMovieRatingsJunction")]
    public class UserMovieRatings
    {
        [Key]
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonIgnore]
        public int UserID { get; set; }
        [JsonProperty("tmdb_id")]
        public int MovieID { get; set; }
        [JsonProperty("rating")]
        public decimal Rating { get; set; }
        [JsonProperty("date_rated")]
        public DateTime Date { get; set; }
    }
}

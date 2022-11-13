using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.UserMovies
{
    [Table("UserLikedMoviesJunction")]
    public class UserLikedMovies
    {
        [Key]
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonIgnore]
        public int UserID { get; set; }
        [JsonProperty("tmdb_id")]
        public int MovieID { get; set; }
        [JsonProperty("date_liked")]
        public DateTime Date { get; set; }
    }
}

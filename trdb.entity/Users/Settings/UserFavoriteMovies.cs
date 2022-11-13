using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Users.Settings
{
    [Table("UserFavoriteMoviesJunction")]
    public class UserFavoriteMovies
    {
        [Key]
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonIgnore]
        public int UserID { get; set; }
        [JsonProperty("tmdb_id")]
        public int MovieID { get; set; }
        [JsonProperty("order")]
        public int SortOrder { get; set; }
        [Write(false)]
        [JsonProperty("title")]
        public string Title { get; set; }
        [Write(false)]
        [JsonProperty("backdrop_path")]
        public string Backdrop_URL { get; set; }
        [Write(false)]
        [JsonProperty("poster_path")]
        public string Poster_URL { get; set; }
    }
}

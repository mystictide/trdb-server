using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.UserFilms
{
    [Table("UserFilmReviewJunction")]
    public class UserFilmReviews
    {
        [Key]
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonIgnore]
        public int UserID { get; set; }
        [JsonProperty("tmdb_id")]
        public int FilmID { get; set; }
        [JsonProperty("rating")]
        public decimal Rating { get; set; }
        [JsonProperty("review")]
        public string? Review { get; set; }
        [JsonProperty("date_watched")]
        public DateTime Date { get; set; }
        [Write(false)]
        [JsonProperty("liked")]
        public bool Liked { get; set; }
    }
}

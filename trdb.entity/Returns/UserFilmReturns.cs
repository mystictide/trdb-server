using Newtonsoft.Json;
using trdb.entity.UserFilms;

namespace trdb.entity.Returns
{
    public class UserFilmReturns
    {
        [JsonProperty("watched")]
        public bool Watched { get; set; }
        [JsonProperty("liked")]
        public bool Liked { get; set; }
        [JsonProperty("watchlist")]
        public bool Watchlist { get; set; }
        [JsonProperty("rating")]
        public UserFilmRatings Rating { get; set; }
        [JsonProperty("reviews")]
        public IEnumerable<UserFilmReviews> Reviews { get; set; }
    }
}

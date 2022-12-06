using Newtonsoft.Json;
using trdb.entity.UserFilms;

namespace trdb.entity.Returns
{
    public class UserReviewReturn
    {
        [JsonProperty("film")]
        public FilmReturn? Film { get; set; }
        [JsonProperty("reviews")]
        public UserFilmReviews? Review { get; set; }
    }
}

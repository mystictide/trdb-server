using Newtonsoft.Json;
using trdb.entity.UserFilms;

namespace trdb.entity.Returns
{
    public class UserLogsReturn
    {
        [JsonProperty("film")]
        public FilmReturn? Film { get; set; }
        [JsonProperty("reviews")]
        public IEnumerable<UserFilmReviews>? Reviews { get; set; }
    }
}

using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Users.Settings
{
    [Table("UserSettingsJunction")]
    public class UserSettings
    {
        [Key]
        [JsonIgnore]
        public int ID { get; set; }
        [JsonIgnore]
        public int UserID { get; set; }
        [JsonProperty("bio")]
        public string Bio { get; set; }
        [JsonProperty("picture_path")]
        public string Picture { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
        [JsonProperty("dmallowed")]
        public bool IsDMAllowed { get; set; }
        [JsonProperty("watchlist_public")]
        public bool IsWatchlistPublic { get; set; }
        [JsonProperty("public")]
        public bool IsPublic { get; set; }
        [JsonProperty("adult")]
        public bool IsAdult { get; set; }
        [Write(false)]
        [JsonProperty("favorite_movies")]
        public List<UserFavoriteMovies> FavoriteMovies { get; set; }
        [Write(false)]
        [JsonProperty("favorite_actors")]
        public List<UserFavoritePeople> FavoriteActors { get; set; }
        [Write(false)]
        [JsonProperty("favorite_directors")]
        public List<UserFavoritePeople> FavoriteDirectors { get; set; }
    }
}
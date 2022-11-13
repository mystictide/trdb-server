using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Users.Settings
{
    [Table("UserFavoritePeopleJunction")]
    public class UserFavoritePeople
    {
        [Key]
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonIgnore]
        public int UserID { get; set; }
        [JsonProperty("tmdb_id")]
        public int PersonID { get; set; }
        [JsonProperty("order")]
        public int SortOrder { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("original_name")]
        public string? Original_Name { get; set; }
        [JsonProperty("profile_path")]
        public string? Photo_URL { get; set; }
    }
}

using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using trdb.entity.Helpers;

namespace trdb.entity.Films
{
    [Table("People")]
    public class People
    {
        [Key]
        public int ID { get; set; }
        [JsonProperty("id")]
        public int TMDB_ID { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("original_name")]
        public string? Original_Name { get; set; }
        [JsonProperty("profile_path")]
        public string? Photo_URL { get; set; }
        [JsonProperty("known_for_department")]
        public string? Profession { get; set; }
        [JsonProperty("gender")]
        public int Gender { get; set; }
        [JsonProperty("adult")]
        public bool IsAdult { get; set; }

        [Write(false)]
        [JsonProperty("character")]
        public string? Character { get; set; }
        [Write(false)]
        [JsonProperty("department")]
        public string? Department { get; set; }
        [Write(false)]
        [JsonProperty("job")]
        public string? Job { get; set; }
        [Write(false)]
        [JsonProperty("order")]
        public int? ListOrder { get; set; }
        [Write(false)]
        [JsonProperty("pager")]
        public Page? Pager { get; set; }
    }
}

using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Movies.Junctions
{
    [Table("MovieCreditsJunction")]
    public class MovieCreditsJunction
    {
        [Key]
        public int ID { get; set; }
        public int MovieID { get; set; }
        public int PersonID { get; set; }
        [JsonProperty("character")]
        public string? Character { get; set; }
        [JsonProperty("department")]
        public string? Department { get; set; }
        [JsonProperty("job")]
        public string? Job { get; set; }
        [JsonProperty("order")]
        public int? ListOrder { get; set; }
    }
}

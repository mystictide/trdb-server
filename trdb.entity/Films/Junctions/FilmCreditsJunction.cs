using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Films.Junctions
{
    [Table("FilmCreditsJunction")]
    public class FilmCreditsJunction
    {
        [Key]
        public int ID { get; set; }
        public int FilmID { get; set; }
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

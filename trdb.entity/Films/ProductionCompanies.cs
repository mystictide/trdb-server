using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Films
{
    [Table("ProductionCompanies")]
    public class ProductionCompanies
    {
        [Key]
        public int ID { get; set; }
        [JsonProperty("id")]
        public int TMDB_ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("logo_path")]
        public string Logo_URL { get; set; }
        [JsonProperty("origin_country")]
        public string Origin { get; set; }
    }
}

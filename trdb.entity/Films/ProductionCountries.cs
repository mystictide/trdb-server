using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Films
{
    [Table("ProductionCountries")]
    public class ProductionCountries
    {
        [Key]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("iso_3166_1")]
        public string iso_3166_1 { get; set; }

    }
}

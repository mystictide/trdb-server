﻿using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Movies
{
    [Table("ProductionCountries")]
    public class ProductionCountries
    {
        [Key]
        [JsonIgnore]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("iso_3166_1")]
        public string iso_3166_1 { get; set; }

    }
}

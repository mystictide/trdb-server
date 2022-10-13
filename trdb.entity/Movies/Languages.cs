﻿using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Movies
{
    [Table("Languages")]
    public class Languages
    {
        [Key]
        public int ID { get; set; }
        [JsonProperty("english_name")]
        public string Name { get; set; }
    }
}

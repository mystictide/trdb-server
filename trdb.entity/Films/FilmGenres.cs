using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace trdb.entity.Films
{
    [Table("FilmGenres")]
    public class FilmGenres
    {
        [Key]
        public int ID { get; set; }
        [JsonProperty("id")]
        public int TMDB_ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [Write(false)]
        [JsonProperty("genres")]
        public List<FilmGenres> List { get; set; }
    }
}

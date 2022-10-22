using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using trdb.entity.Helpers;

namespace trdb.entity.Movies
{
    [Table("Movies")]
    public class Movies
    {
        [Key]
        public int ID { get; set; }
        [JsonProperty("id")]
        public int TMDB_ID { get; set; }
        [JsonProperty("imdb_id")]
        public string IMDB_ID { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("budget")]
        public decimal Budget { get; set; }
        [JsonProperty("backdrop_path")]
        public string Backdrop_URL { get; set; }
        [JsonProperty("poster_path")]
        public string Poster_URL { get; set; }
        [JsonProperty("homepage")]
        public string Homepage { get; set; }
        [JsonProperty("overview")]
        public string Synopsis { get; set; }
        [JsonProperty("runtime")]
        public int Runtime { get; set; }
        [JsonProperty("release_date")]
        public string Release_Date { get; set; }
        [JsonProperty("tagline")]
        public string Tagline { get; set; }
        [JsonProperty("adult")]
        public bool IsAdult { get; set; }

        public Movies()
        {
            Genres = new List<MovieGenres>();
            Languages = new List<Languages>();
            Companies = new List<ProductionCompanies>();
            Countries = new List<ProductionCountries>();
            Credits = new Credits();
        }

        [Write(false)]
        [JsonProperty("genres")]
        public List<MovieGenres> Genres { get; set; }
        [Write(false)]
        [JsonProperty("spoken_languages")]
        public List<Languages> Languages { get; set; }
        [Write(false)]
        [JsonProperty("production_companies")]
        public List<ProductionCompanies> Companies { get; set; }
        [Write(false)]
        [JsonProperty("production_countries")]
        public List<ProductionCountries> Countries { get; set; }
        //[Write(false)]
        //[JsonProperty("credits")]
        //public List<People> People { get; set; }
        [Write(false)]
        [JsonProperty("credits")]
        public Credits Credits { get; set; }
        [Write(false)]
        [JsonProperty("expires")]
        public DateTime WeeklyExpiryDate { get; set; }
    }
}

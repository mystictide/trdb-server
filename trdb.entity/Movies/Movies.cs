using Dapper.Contrib.Extensions;

namespace trdb.entity.Movies
{
    [Table("Movies")]
    public class Movies
    {
        [Key]
        public int ID { get; set; }
        public int TMDB_ID { get; set; }
        public string IMDB_ID { get; set; }
        public string Title { get; set; }
        public decimal Budget { get; set; }
        public string Backdrop_URL { get; set; }
        public string Poster_URL { get; set; }
        public string Homepage { get; set; }
        public string Synopsis { get; set; }
        public int Runtime { get; set; }
        public string Release_Date { get; set; }
        public string Tagline { get; set; }
        public bool IsAdult { get; set; }
    }
}

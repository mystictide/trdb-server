using Dapper.Contrib.Extensions;

namespace trdb.entity.Movies.Junctions
{
    [Table("MovieGenreJunction")]
    public class MovieGenreJunction
    {
        [Key]
        public int ID { get; set; }
        public int MovieID { get; set; }
        public int GenreID { get; set; }
    }
}

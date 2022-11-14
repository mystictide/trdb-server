using Dapper.Contrib.Extensions;

namespace trdb.entity.Films.Junctions
{
    [Table("FilmGenreJunction")]
    public class FilmGenreJunction
    {
        [Key]
        public int ID { get; set; }
        public int FilmID { get; set; }
        public int GenreID { get; set; }
    }
}

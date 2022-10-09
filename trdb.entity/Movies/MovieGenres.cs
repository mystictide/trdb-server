using Dapper.Contrib.Extensions;

namespace trdb.entity.Movies
{
    [Table("MovieGenres")]
    public class MovieGenres
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

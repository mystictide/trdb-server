using Dapper.Contrib.Extensions;

namespace trdb.entity.Films.Junctions
{
    [Table("FilmLanguageJunction")]
    public class FilmLanguageJunction
    {
        [Key]
        public int ID { get; set; }
        public int FilmID { get; set; }
        public int LanguageID { get; set; }
    }
}

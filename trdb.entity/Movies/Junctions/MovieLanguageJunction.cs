using Dapper.Contrib.Extensions;

namespace trdb.entity.Movies.Junctions
{
    [Table("MovieLanguageJunction")]
    public class MovieLanguageJunction
    {
        [Key]
        public int ID { get; set; }
        public int MovieID { get; set; }
        public int LanguageID { get; set; }

        [Write(false)]
        public string Name { get; set; }
    }
}

using Dapper.Contrib.Extensions;

namespace trdb.entity.Movies.Junctions
{
    [Table("MovieProductionCountryJunction")]
    public class MovieProductionCountryJunction
    {
        [Key]
        public int ID { get; set; }
        public int MovieID { get; set; }
        public int ProductionCountryID { get; set; }

        [Write(false)]
        public string Name { get; set; }
    }
}

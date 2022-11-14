using Dapper.Contrib.Extensions;

namespace trdb.entity.Films.Junctions
{
    [Table("FilmProductionCountryJunction")]
    public class FilmProductionCountryJunction
    {
        [Key]
        public int ID { get; set; }
        public int FilmID { get; set; }
        public int ProductionCountryID { get; set; }
    }
}

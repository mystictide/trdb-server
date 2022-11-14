using Dapper.Contrib.Extensions;


namespace trdb.entity.Films.Junctions
{
    [Table("FilmProductionCompanyJunction")]
    public class FilmProductionCompanyJunction
    {
        [Key]
        public int ID { get; set; }
        public int FilmID { get; set; }
        public int ProductionCompanyID { get; set; }
    }
}

using Dapper.Contrib.Extensions;


namespace trdb.entity.Movies.Junctions
{
    [Table("MovieProductionCompanyJunction")]
    public class MovieProductionCompanyJunction
    {
        [Key]
        public int ID { get; set; }
        public int MovieID { get; set; }
        public int ProductionCompanyID { get; set; }
    }
}

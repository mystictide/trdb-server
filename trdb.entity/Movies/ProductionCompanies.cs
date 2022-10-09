using Dapper.Contrib.Extensions;

namespace trdb.entity.Movies
{
    [Table("ProductionCompanies")]
    public class ProductionCompanies
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Logo_URL { get; set; }
        public string Origin { get; set; }
    }
}

using Dapper.Contrib.Extensions;

namespace trdb.entity.Movies
{
    [Table("Languages")]
    public class Languages
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

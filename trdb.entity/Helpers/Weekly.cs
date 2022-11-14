using Dapper.Contrib.Extensions;

namespace trdb.entity.Helpers
{
    [Table("Weekly")]
    public class Weekly
    {
        [Key]
        public int ID { get; set; }
        public int FilmID { get; set; }
        public DateTime Date{ get; set; }
    }
}

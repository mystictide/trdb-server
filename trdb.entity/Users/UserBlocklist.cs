using Dapper.Contrib.Extensions;

namespace trdb.entity.Users
{
    [Table("UserBlocklistJunction")]
    public class UserBlocklist
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int BlockerID { get; set; }
    }
}

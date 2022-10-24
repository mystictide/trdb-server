using Dapper.Contrib.Extensions;

namespace trdb.entity.Users
{
    [Table("UserFollowsJunction")]
    public class UserFollows
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int FollowerID { get; set; }
        public DateTime Date { get; set; }
    }
}

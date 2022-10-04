using Dapper.Contrib.Extensions;

namespace trdb.entity.Users
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int? ID { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? AuthType { get; set; }
        public bool? IsActive { get; set; }
    }
}

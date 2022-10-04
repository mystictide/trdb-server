using System.Data.SqlClient;
using System.Data;

namespace trdb.data.Connection
{
    internal class dbConnectionExample
    {
        private static string? connectionString { get; set; }

        public dbConnectionExample()
        {
            connectionString = "Server=.\\SQL;Database=database;User Id=username;Password=password;";
        }

        public static IDbConnection GetConnection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
    }
}

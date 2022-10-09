namespace trdb.entity.Helpers
{
    public class AppSettings
    {
        private static string _secret;
        public static string Secret
        {
            get
            {
                return "Orpheus Under the Influence";
            }
            set
            {
                _secret = value;
            }
        }
    }
}

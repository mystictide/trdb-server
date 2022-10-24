namespace trdb.api.Models
{
    public class TMDB_Pager
    {
        public int page { get; set; }
        public List<object> results { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
    }
}

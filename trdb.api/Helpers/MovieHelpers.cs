using Newtonsoft.Json;
using trdb.entity.Movies;

namespace trdb.api.Helpers
{
    public class MovieHelpers
    {
        public static Movies FormatTMDBResponse(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<Movies>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

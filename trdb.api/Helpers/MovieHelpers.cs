using Newtonsoft.Json;
using trdb.entity.Movies;

namespace trdb.api.Helpers
{
    public class MovieHelpers
    {
        public static MovieGenres FormatTMDBGenresResponse(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<MovieGenres>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<Languages> FormatTMDBLanguagesResponse(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<Languages>>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<ProductionCountries> FormatTMDBCountryResponse(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<ProductionCountries>>(jsonString);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Movies FormatTMDBMovieResponse(string jsonString)
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

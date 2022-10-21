using Newtonsoft.Json;
using RestSharp;
using trdb.entity.Helpers;
using trdb.entity.Movies;

namespace trdb.api.Helpers
{
    public class MovieHelpers
    {
        private static string tmdb_key = "c33e76a04be19de0f46ae6301aec3a6a";
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

        public static async Task<Movies> FormatTMDBMovieResponse(string jsonString)
        {
            try
            {
                var movie = JsonConvert.DeserializeObject<Movies>(jsonString);
                movie.Credits = await FormatTMDBCreditsResponse(movie.TMDB_ID);
                return movie;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<Credits> FormatTMDBCreditsResponse(int MovieID)
        {
            try
            {
                var url = "https://api.themoviedb.org/3/movie/+" + MovieID + "/credits?api_key=" + tmdb_key + "&language=en-US";
                var response = await CustomHelpers.SendRequest(url, Method.Get);
                var credits = JsonConvert.DeserializeObject<Credits>(response);
                return credits;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

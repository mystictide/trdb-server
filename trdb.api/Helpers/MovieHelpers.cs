using Newtonsoft.Json;
using RestSharp;
using trdb.api.Models;
using trdb.entity.Helpers;
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

        public static async Task<MovieReturn> FormatTMDBSimpleMovieResponse(string jsonString)
        {
            try
            {
                var movie = JsonConvert.DeserializeObject<MovieReturn>(jsonString);
                return movie;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<MovieReturn>> FormatTMDBSimpleMovieListResponse(string jsonString)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<TMDB_Pager>(jsonString);
                var movies = CustomHelpers.CastObjectsAsSimpleMovies(response.results);
                return movies;
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
                var url = "https://api.themoviedb.org/3/movie/+" + MovieID + "/credits?api_key=" + CustomHelpers.tmdb_key + "&language=en-US";
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

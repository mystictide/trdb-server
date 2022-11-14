using Newtonsoft.Json;
using RestSharp;
using trdb.api.Models;
using trdb.entity.Helpers;
using trdb.entity.Films;
using trdb.entity.Returns;

namespace trdb.api.Helpers
{
    public class FilmHelpers
    {
        public static FilmGenres FormatTMDBGenresResponse(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<FilmGenres>(jsonString);
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

        public static async Task<Films> FormatTMDBFilmResponse(string jsonString)
        {
            try
            {
                var Film = JsonConvert.DeserializeObject<Films>(jsonString);
                Film.Credits = await FormatTMDBCreditsResponse(Film.TMDB_ID);
                return Film;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<FilmReturn> FormatTMDBSimpleFilmResponse(string jsonString)
        {
            try
            {
                var Film = JsonConvert.DeserializeObject<FilmReturn>(jsonString);
                return Film;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<FilmReturn>> FormatTMDBSimpleFilmListResponse(string jsonString)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<TMDB_Pager>(jsonString);
                var Films = CustomHelpers.CastObjectsAsSimpleFilms(response.results);
                return Films;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<Credits> FormatTMDBCreditsResponse(int FilmID)
        {
            try
            {
                var url = "https://api.themoviedb.org/3/movie/+" + FilmID + "/credits?api_key=" + CustomHelpers.tmdb_key + "&language=en-US";
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

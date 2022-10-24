using Newtonsoft.Json;
using RestSharp;
using trdb.api.Models;
using trdb.entity.Movies;
using trdb.entity.Users;

namespace trdb.api.Helpers
{
    public class CustomHelpers
    {
        public static string tmdb_key = "c33e76a04be19de0f46ae6301aec3a6a";
        public static async Task<string> SendRequest(string url, Method method)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(url, method);
                RestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode != System.Net.HttpStatusCode.NotModified)
                {
                    return JsonConvert.DeserializeObject(response.Content).ToString();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsResponseSuccessful(string jsonString)
        {
            try
            {
                var status = JsonConvert.DeserializeObject<TMDB_Error>(jsonString);
                if (status.Code > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static WeeklyReturn CastMovieAsWeekly(Movies obj)
        {
            return JsonConvert.DeserializeObject<WeeklyReturn>(JsonConvert.SerializeObject(obj));
        }
        public static List<Movies> CastObjectsAsMovies(List<object> obj)
        {
            return JsonConvert.DeserializeObject<List<Movies>>(JsonConvert.SerializeObject(obj));
        }

        public static List<UserReturn> CastUsersAsUserReturns(List<Users> obj)
        {
            return JsonConvert.DeserializeObject<List<UserReturn>>(JsonConvert.SerializeObject(obj));
        }

        public static List<MovieReturn> CastObjectsAsSimpleMovies(List<object> obj)
        {
            return JsonConvert.DeserializeObject<List<MovieReturn>>(JsonConvert.SerializeObject(obj));
        }
    }
}

using Newtonsoft.Json;
using RestSharp;
using trdb.api.Models;

namespace trdb.api.Helpers
{
    public class CustomHelpers
    {
        public static async Task<string> SendRequest(string url, Method method)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(url, method);
                RestResponse response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject(response.Content).ToString();
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
    }
}

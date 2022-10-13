using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using trdb.api.Helpers;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("tmdb")]
    public class TMDBController : Controller
    {
        private readonly ILogger<TMDBController> _logger;

        public TMDBController(ILogger<TMDBController> logger)
        {
            _logger = logger;
        }

        private static int AuthorizedAuthType = 3;
        private static string tmdb_key = "c33e76a04be19de0f46ae6301aec3a6a";

        #region List
        #endregion

        #region Import
        [HttpPost]
        [Route("import/genre")]
        public async Task<IActionResult> ImportGenres()
        {
            try
            {
                var token = TokenHelpers.ReadBearerToken(HttpContext);
                if (TokenHelpers.ValidateToken(token, AuthorizedAuthType))
                {

                }
                return Ok("");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("import/movie")]
        public async Task<IActionResult> ImportMovies()
        {
            try
            {
                var token = TokenHelpers.ReadBearerToken(HttpContext);
                if (TokenHelpers.ValidateToken(token, AuthorizedAuthType))
                {
                    var url = "https://api.themoviedb.org/3/movie/2?api_key=" + tmdb_key;
                    var client = new RestClient(url);
                    var request = new RestRequest(url, Method.Get);
                    RestResponse response = await client.ExecuteAsync(request);
                    var jsonResponse = JsonConvert.DeserializeObject(response.Content);
                    dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Content);

                    if (CustomHelpers.IsResponseSuccessful(data))
                    {
                        var movieData = MovieHelpers.FormatTMDBResponse(jsonResponse.ToString());
                        return Ok(data);
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion
    }
}

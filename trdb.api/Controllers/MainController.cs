using Microsoft.AspNetCore.Mvc;
using RestSharp;
using trdb.api.Helpers;
using trdb.business.Helpers;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("main")]
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        #region homepage

        [HttpGet]
        [Route("get/weekly")]
        public async Task<IActionResult> GetWeekly()
        {
            try
            {
                var movie = await new WeeklyManager().Manage();
                var result = CustomHelpers.CastMovieAsWeekly(movie);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/popular")]
        public async Task<IActionResult> GetPopular()
        {
            try
            {
                var url = "https://api.themoviedb.org/3/movie/popular?api_key=" + CustomHelpers.tmdb_key + "&language=en-US&page=1";
                var response = await CustomHelpers.SendRequest(url, Method.Get);

                if (CustomHelpers.IsResponseSuccessful(response))
                {
                    var result = await MovieHelpers.FormatTMDBSimpleMovieListResponse(response);
                    return Ok(result.Take(6));
                }
                return StatusCode(500, "No items found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/top")]
        public async Task<IActionResult> GetTopRated()
        {
            try
            {
                var url = "https://api.themoviedb.org/3/movie/top_rated?api_key=" + CustomHelpers.tmdb_key + "&language=en-US&page=1";
                var response = await CustomHelpers.SendRequest(url, Method.Get);

                if (CustomHelpers.IsResponseSuccessful(response))
                {
                    var result = await MovieHelpers.FormatTMDBSimpleMovieListResponse(response);
                    return Ok(result.Take(6));
                }
                return StatusCode(500, "No items found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using RestSharp;
using trdb.api.Helpers;
using trdb.business.Helpers;
using trdb.business.Films;
using trdb.entity.Helpers;
using trdb.entity.Films;
using trdb.entity.Returns;

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

        [HttpGet]
        [Route("browse")]
        public async Task<IActionResult> Browse([FromQuery] Filter filter)
        {
            try
            {
                var result = new Browser();
                var filterFilms = new Films();
                var filterPeople = new People();
                filter.pageSize = 15;
                filter.isDetailSearch = false;
                FilteredList<Films> request = new FilteredList<Films>()
                {
                    filter = filter,
                    filterModel = filterFilms,
                };
                result.Films = await new FilmManager().FilteredList(request);
                FilteredList<People> requestp = new FilteredList<People>()
                {
                    filter = filter,
                    filterModel = filterPeople,
                };
                result.People = await new PeopleManager().FilteredList(requestp);
                //user lists
                //users
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("search/film")]
        public async Task<IActionResult> SearchFilm([FromQuery] Filter filter)
        {
            try
            {
                var filterModel = new Films();
                filter.pageSize = 20;
                filter.isDetailSearch = false;
                FilteredList<Films> request = new FilteredList<Films>()
                {
                    filter = filter,
                    filterModel = filterModel,
                };
                var result = await new FilmManager().FilteredList(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("search/actor")]
        public async Task<IActionResult> SearchActor([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, 1))
                {
                    var filterModel = new People();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
                    filterModel.Profession = "Acting";
                    FilteredList<People> request = new FilteredList<People>()
                    {
                        filter = filter,
                        filterModel = filterModel,
                    };
                    var result = await new PeopleManager().FilteredList(request);
                    return Ok(result);
                }

                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("search/director")]
        public async Task<IActionResult> SearchDirector([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, 1))
                {
                    var filterModel = new People();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
                    filterModel.Profession = "Directing";
                    FilteredList<People> request = new FilteredList<People>()
                    {
                        filter = filter,
                        filterModel = filterModel,
                    };
                    var result = await new PeopleManager().FilteredList(request);
                    return Ok(result);
                }

                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #region homepage

        [HttpGet]
        [Route("get/weekly")]
        public async Task<IActionResult> GetWeekly()
        {
            try
            {
                var Film = await new WeeklyManager().Manage();
                var result = CustomHelpers.CastFilmAsWeekly(Film);
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
                    var result = await FilmHelpers.FormatTMDBSimpleFilmListResponse(response);
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
                    var result = await FilmHelpers.FormatTMDBSimpleFilmListResponse(response);
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

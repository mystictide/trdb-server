using Microsoft.AspNetCore.Mvc;
using RestSharp;
using trdb.api.Helpers;
using trdb.business.Movies;
using trdb.entity.Helpers;
using trdb.entity.Movies;
using trdb.entity.Users;

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

        #region Get
        [HttpGet]
        [Route("get/movie")]
        public async Task<IActionResult> GetMovie([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new MovieManager().Get(ID);
                    return Ok(result);
                }

                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion


        #region List

        [HttpGet]
        [Route("list/genres")]
        public async Task<IActionResult> ListGenres([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var filterModel = new MovieGenres();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
                    FilteredList<MovieGenres> request = new FilteredList<MovieGenres>()
                    {
                        filter = filter,
                        filterModel = filterModel,
                    };
                    var result = await new MovieGenreManager().FilteredList(request);
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
        [Route("list/langs")]
        public async Task<IActionResult> ListLanguages([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var filterModel = new Languages();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
                    FilteredList<Languages> request = new FilteredList<Languages>()
                    {
                        filter = filter,
                        filterModel = filterModel,
                    };
                    var result = await new LanguageManager().FilteredList(request);
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
        [Route("list/companies")]
        public async Task<IActionResult> ListCompanies([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var filterModel = new ProductionCompanies();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
                    FilteredList<ProductionCompanies> request = new FilteredList<ProductionCompanies>()
                    {
                        filter = filter,
                        filterModel = filterModel,
                    };
                    var result = await new ProductionCompanyManager().FilteredList(request);
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
        [Route("list/countries")]
        public async Task<IActionResult> ListCountries([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var filterModel = new ProductionCountries();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
                    FilteredList<ProductionCountries> request = new FilteredList<ProductionCountries>()
                    {
                        filter = filter,
                        filterModel = filterModel,
                    };
                    var result = await new ProductionCountryManager().FilteredList(request);
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
        [Route("list/movies")]
        public async Task<IActionResult> ListMovies([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var filterModel = new Movies();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
                    FilteredList<Movies> request = new FilteredList<Movies>()
                    {
                        filter = filter,
                        filterModel = filterModel,
                    };
                    var result = await new MovieManager().FilteredList(request);
                    return Ok(result);
                }

                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

        #region Import

        [HttpPost]
        [Route("import/genres")]
        public async Task<IActionResult> ImportGenres()
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var url = "https://api.themoviedb.org/3/genre/movie/list?api_key=" + tmdb_key + "&language=en-US";
                    var response = await CustomHelpers.SendRequest(url, Method.Get);

                    if (CustomHelpers.IsResponseSuccessful(response))
                    {
                        var genres = MovieHelpers.FormatTMDBGenresResponse(response);
                        var import = await new MovieGenreManager().Import(genres.List);
                        return Ok(import);
                    }
                }

                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("import/langs")]
        public async Task<IActionResult> ImportLanguages()
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var url = "https://api.themoviedb.org/3/configuration/languages?api_key=" + tmdb_key;
                    var response = await CustomHelpers.SendRequest(url, Method.Get);

                    if (CustomHelpers.IsResponseSuccessful(response))
                    {
                        var langs = MovieHelpers.FormatTMDBLanguagesResponse(response);
                        var import = await new LanguageManager().Import(langs);
                        return Ok(import);
                    }
                }

                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("import/countries")]
        public async Task<IActionResult> ImportCountries()
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var url = "https://api.themoviedb.org/3/configuration/countries?api_key=" + tmdb_key;
                    var response = await CustomHelpers.SendRequest(url, Method.Get);

                    if (CustomHelpers.IsResponseSuccessful(response))
                    {
                        var countries = MovieHelpers.FormatTMDBCountryResponse(response);
                        var import = await new ProductionCountryManager().Import(countries);
                        return Ok(import);
                    }
                }

                return StatusCode(500, "Authorization failed");
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
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var import = new Movies();
                    var movieID = await new MovieManager().GetLatestMovie() + 1;

                    while (import.TMDB_ID < 1)
                    {
                        var url = "https://api.themoviedb.org/3/movie/" + movieID + "?api_key=" + tmdb_key;
                        var response = await CustomHelpers.SendRequest(url, Method.Get);

                        if (CustomHelpers.IsResponseSuccessful(response))
                        {
                            var movieData = MovieHelpers.FormatTMDBMovieResponse(response);
                            import = await new MovieManager().Import(movieData);
                        }
                        else
                        {
                            movieID++;
                        }
                    }
                    return Ok(import);
                }

                return StatusCode(500, "Authorization failed");
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

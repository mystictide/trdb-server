using RestSharp;
using Microsoft.AspNetCore.Mvc;
using trdb.api.Helpers;
using trdb.business.Films;
using trdb.entity.Helpers;
using trdb.entity.Films;

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

        #region Get

        [HttpGet]
        [Route("get/genre")]
        public async Task<IActionResult> GetGenre([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new FilmGenreManager().Get(ID);
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
        [Route("get/language")]
        public async Task<IActionResult> GetLanguage([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new LanguageManager().Get(ID);
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
        [Route("get/company")]
        public async Task<IActionResult> GetCompany([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new ProductionCompanyManager().Get(ID);
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
        [Route("get/country")]
        public async Task<IActionResult> GetCountry([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new ProductionCountryManager().Get(ID);
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
        [Route("get/film")]
        public async Task<IActionResult> GetFilm([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new FilmManager().Get(ID);
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
        [Route("list/people")]
        public async Task<IActionResult> ListPeople([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var filterModel = new People();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
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
        [Route("list/genres")]
        public async Task<IActionResult> ListGenres([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var filterModel = new FilmGenres();
                    filter.pageSize = 20;
                    filter.isDetailSearch = false;
                    FilteredList<FilmGenres> request = new FilteredList<FilmGenres>()
                    {
                        filter = filter,
                        filterModel = filterModel,
                    };
                    var result = await new FilmGenreManager().FilteredList(request);
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
        [Route("list/film")]
        public async Task<IActionResult> ListFilms([FromQuery] Filter filter)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
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
                    var url = "https://api.themoviedb.org/3/genre/movie/list?api_key=" + CustomHelpers.tmdb_key + "&language=en-US";
                    var response = await CustomHelpers.SendRequest(url, Method.Get);

                    if (CustomHelpers.IsResponseSuccessful(response))
                    {
                        var genres = FilmHelpers.FormatTMDBGenresResponse(response);
                        var import = await new FilmGenreManager().Import(genres.List);
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
                    var url = "https://api.themoviedb.org/3/configuration/languages?api_key=" + CustomHelpers.tmdb_key;
                    var response = await CustomHelpers.SendRequest(url, Method.Get);

                    if (CustomHelpers.IsResponseSuccessful(response))
                    {
                        var langs = FilmHelpers.FormatTMDBLanguagesResponse(response);
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
                    var url = "https://api.themoviedb.org/3/configuration/countries?api_key=" + CustomHelpers.tmdb_key;
                    var response = await CustomHelpers.SendRequest(url, Method.Get);

                    if (CustomHelpers.IsResponseSuccessful(response))
                    {
                        var countries = FilmHelpers.FormatTMDBCountryResponse(response);
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
        [Route("import/film")]
        public async Task<IActionResult> ImportFilms([FromQuery] int? FilmID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var import = new Films();
                    if (FilmID == null)
                    {
                        FilmID = await new FilmManager().GetLatestFilm() + 1;
                    }

                    while (import.TMDB_ID < 1)
                    {
                        var url = "https://api.themoviedb.org/3/movie/" + FilmID + "?api_key=" + CustomHelpers.tmdb_key;
                        var response = await CustomHelpers.SendRequest(url, Method.Get);

                        if (response != null && CustomHelpers.IsResponseSuccessful(response))
                        {
                            var FilmData = await FilmHelpers.FormatTMDBFilmResponse(response);
                            import = await new FilmManager().Import(FilmData);
                        }
                        else
                        {
                            FilmID++;
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

using Microsoft.AspNetCore.Mvc;
using RestSharp;
using trdb.api.Helpers;
using trdb.business.Movies;
using trdb.entity.Movies;

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
                        var url = "https://api.themoviedb.org/3/movie/" + 2 + "?api_key=" + tmdb_key;
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

using Microsoft.AspNetCore.Mvc;
using trdb.api.Helpers;
using trdb.business.Films;
using trdb.business.UserFilms;
using trdb.entity.UserFilms;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("film")]
    public class FilmController : Controller
    {
        private static int AuthorizedAuthType = 1;

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetFilm([FromQuery] int? ID, [FromQuery] string? title)
        {
            try
            {
                var result = await new FilmManager().GetFilmDetails(ID, title);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/credits/cast")]
        public async Task<IActionResult> GetFilmCast([FromQuery] int ID)
        {
            try
            {
                var result = await new PeopleManager().GetCast(ID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/credits/crew")]
        public async Task<IActionResult> GetFilmCrew([FromQuery] int ID)
        {
            try
            {
                var result = await new PeopleManager().GetCrew(ID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/user")]
        public async Task<IActionResult> GetUserFilmDetails([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserFilmManager().GetUserFilmDetails(ID, AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("watch")]
        public async Task<IActionResult> WatchFilm([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserFilmManager().ToggleWatched(ID, AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("like")]
        public async Task<IActionResult> LikeFilm([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserFilmManager().ToggleLike(ID, AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("watchlist")]
        public async Task<IActionResult> WatchlistFilm([FromQuery] int ID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserFilmManager().ToggleWatchlist(ID, AuthHelpers.CurrentUserID(HttpContext));
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("rate")]
        public async Task<IActionResult> ManageRating([FromBody] UserFilmRatings entity)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserFilmManager().ManageRatings(entity, AuthHelpers.CurrentUserID(HttpContext));
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("review")]
        public async Task<IActionResult> ManageReview([FromBody] UserFilmReviews entity)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserFilmManager().ManageReview(entity, AuthHelpers.CurrentUserID(HttpContext));
                    return Ok(result);
                }
                return StatusCode(500, "Authorization failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

using trdb.api.Helpers;
using trdb.entity.UserFilms;
using trdb.business.UserFilms;
using Microsoft.AspNetCore.Mvc;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("film")]
    public class UserFilmsController : Controller
    {
        private static int AuthorizedAuthType = 1;

        [HttpGet]
        [Route("get/user/logs")]
        public async Task<IActionResult> GetUserFilmReview([FromQuery] string username, [FromQuery] string title, [FromQuery] string year)
        {
            try
            {
                var result = await new UserFilmManager().GetUserFilmLogs(username, title, year);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/user/review")]
        public async Task<IActionResult> GetUserFilmReview([FromQuery] string username, [FromQuery] string title, [FromQuery] string year, [FromQuery] int? count)
        {
            try
            {
                var result = await new UserFilmManager().GetUserFilmReview(username, title, year, count);
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

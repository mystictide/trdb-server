using Microsoft.AspNetCore.Mvc;
using trdb.business.Users;
using trdb.entity.Returns;
using trdb.api.Helpers;
using trdb.entity.Users.Settings;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("settings")]
    public class UserSettingsController : Controller
    {
        private IWebHostEnvironment _env;

        public UserSettingsController(IWebHostEnvironment env)
        {
            _env = env;
        }
        private static int AuthorizedAuthType = 1;

        [HttpPost]
        [Route("personal")]
        public async Task<IActionResult> UpdatePersonal([FromBody] SettingsReturn entity)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserManager().UpdatePersonalSettings(entity, AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("avatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile file)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    string result = "";
                    if (file.Length > 0)
                    {
                        var path = await CustomHelpers.SaveUserAvatar(AuthHelpers.CurrentUserID(HttpContext), _env.ContentRootPath, file);
                        if (path != null)
                        {
                            result = await new UserManager().UpdateAvatar(path, AuthHelpers.CurrentUserID(HttpContext));
                        }
                        else
                        {
                            return StatusCode(401, "Failed to save image");
                        }
                    }
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
        [Route("favorites/films")]
        public async Task<IActionResult> ManageFavoriteFilms([FromBody] List<UserFavoriteFilms> entity)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserManager().ManageFavoriteFilms(entity, AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("favorites/actors")]
        public async Task<IActionResult> ManageFavoriteActors([FromBody] List<UserFavoritePeople> entity)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserManager().ManageFavoriteActors(entity, AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("favorites/directors")]
        public async Task<IActionResult> ManageFavoriteDirectors([FromBody] List<UserFavoritePeople> entity)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserManager().ManageFavoriteDirectors(entity, AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("dm")]
        public async Task<IActionResult> ToggleDMs()
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserManager().ToggleDMs(AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("watchlist")]
        public async Task<IActionResult> ToggleWatchlist()
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserManager().ToggleWatchlist(AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("privacy")]
        public async Task<IActionResult> TogglePrivacy()
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserManager().TogglePrivacy(AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("adult")]
        public async Task<IActionResult> ToggleAdultContent()
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, AuthorizedAuthType))
                {
                    var result = await new UserManager().ToggleAdultContent(AuthHelpers.CurrentUserID(HttpContext));
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

using Microsoft.AspNetCore.Mvc;
using trdb.api.Helpers;
using trdb.business.Users;
using trdb.entity.Users;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("settings")]
    public class UserSettingsController : Controller
    {
        private readonly ILogger<UserSettingsController> _logger;

        public UserSettingsController(ILogger<UserSettingsController> logger)
        {
            _logger = logger;
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
        public async Task<IActionResult> UpdateAvatar([FromBody] SettingsReturn entity)
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

using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using trdb.api.Helpers;
using trdb.business.Users;
using trdb.entity.Users;
using static System.Net.WebRequestMethods;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("settings")]
    public class UserSettingsController : Controller
    {
        private readonly ILogger<UserSettingsController> _logger;
        private IWebHostEnvironment _env;

        public UserSettingsController(ILogger<UserSettingsController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
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
                        AuthHelpers.CurrentUserID(HttpContext);
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

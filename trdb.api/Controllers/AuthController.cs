using Microsoft.AspNetCore.Mvc;
using trdb.api.Helpers;
using trdb.business.Users;
using trdb.entity.Returns;
using trdb.entity.Users;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            try
            {
                var data = await new UserManager().Register(user);
                var userData = new UserReturn();
                userData.Username = data.Username;
                userData.Email = data.Email;
                userData.Token = data.Token;
                userData.Settings = data.Settings;
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Users user)
        {
            try
            {
                var data = await new UserManager().Login(user);
                var userData = new UserReturn();
                userData.ID = data.ID;
                userData.Username = data.Username;
                userData.Email = data.Email;
                userData.Token = data.Token;
                userData.Settings = data.Settings;
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("cmail")]
        public async Task<IActionResult> CheckExistingEmail([FromBody] string email)
        {
            try
            {
                bool exists;
                var userID = AuthHelpers.CurrentUserID(HttpContext);
                if (userID < 1)
                {
                    exists = await new UserManager().CheckEmail(email, null);
                }
                else
                {
                    exists = await new UserManager().CheckEmail(email, userID);
                }
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("cusername")]
        public async Task<IActionResult> checkExistingUsername([FromBody] string username)
        {
            try
            {
                bool exists;
                var userID = AuthHelpers.CurrentUserID(HttpContext);
                if (userID < 1)
                {
                    exists = await new UserManager().CheckUsername(username, null);
                }
                else
                {
                    exists = await new UserManager().CheckUsername(username, userID);
                }

                return Ok(exists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

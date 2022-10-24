using Microsoft.AspNetCore.Mvc;
using trdb.api.Helpers;
using trdb.business.Users;
using trdb.entity.Users;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("get/user")]
        public async Task<IActionResult> GetUser([FromQuery] int? UserID, [FromQuery] string? Username)
        {
            try
            {
                var data = await new UserManager().Get(UserID, Username);
                var user = new UserReturn();
                user.Username = data.Username;
                user.Token = data.Token;
                user.Settings = data.Settings;
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/following")]
        public async Task<IActionResult> GetFollowing([FromQuery] int UserID)
        {
            try
            {
                var data = await new UserManager().GetFollowing(UserID);
                var result = CustomHelpers.CastUsersAsUserReturns(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/followers")]
        public async Task<IActionResult> GetFollowers([FromQuery] int UserID)
        {
            try
            {
                var data = await new UserManager().GetFollowers(UserID);
                var result = CustomHelpers.CastUsersAsUserReturns(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/blocklist")]
        public async Task<IActionResult> GetBlocklist([FromQuery] int UserID)
        {
            try
            {
                var data = await new UserManager().GetBlocklist(UserID);
                var result = CustomHelpers.CastUsersAsUserReturns(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

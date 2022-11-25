using Microsoft.AspNetCore.Mvc;
using trdb.api.Helpers;
using trdb.business.Users;
using trdb.entity.Returns;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }


        #region interactions
        [HttpPost]
        [Route("follow")]
        public async Task<IActionResult> FollowUser([FromBody] int targetID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, 1))
                {
                    var result = await new UserManager().Follow(targetID, AuthHelpers.CurrentUserID(HttpContext));
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
        [Route("block")]
        public async Task<IActionResult> BlockUser([FromBody] int targetID)
        {
            try
            {
                if (AuthHelpers.Authorize(HttpContext, 1))
                {
                    var result = await new UserManager().Block(targetID, AuthHelpers.CurrentUserID(HttpContext));
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

        #region get 

        [HttpGet]
        [Route("get/user")]
        public async Task<IActionResult> GetUser([FromQuery] int? UserID, [FromQuery] string? Username)
        {
            try
            {
                var data = await new UserManager().Get(UserID, Username);
                var user = new UserReturn();
                user.ID = data.ID;
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
        #endregion
    }
}

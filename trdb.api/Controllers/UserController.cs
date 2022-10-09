using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using trdb.business.Users;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("user"), Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userData = await new UserManager().Get(1);
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

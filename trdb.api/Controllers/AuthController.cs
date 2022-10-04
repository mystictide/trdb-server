using Microsoft.AspNetCore.Mvc;
using trdb.business.Users;
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

        [HttpGet]
        [Route("register")]
        public string Register()
        {
            return "success";
        }

        [HttpPost]
        [Route("login")]
        public async Task<string> Login([FromBody] Users user)
        {
            await new UserManager().Login(user.Email);
            return "success";
        }
    }
}

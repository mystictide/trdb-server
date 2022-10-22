using Microsoft.AspNetCore.Mvc;
using trdb.business.Helpers;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("main")]
    public class MainController : Controller
    {
        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("get/weekly")]
        public async Task<IActionResult> GetWeekly()
        {
            try
            {
                var result = await new WeeklyManager().Manage();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

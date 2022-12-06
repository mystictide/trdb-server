using Microsoft.AspNetCore.Mvc;
using trdb.business.Films;

namespace trdb.api.Controllers
{
    [ApiController]
    [Route("film")]
    public class FilmController : Controller
    {
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetFilm([FromQuery] int? ID, [FromQuery] string? title)
        {
            try
            {
                var result = await new FilmManager().GetFilmDetails(ID, title);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/credits/cast")]
        public async Task<IActionResult> GetFilmCast([FromQuery] int ID)
        {
            try
            {
                var result = await new PeopleManager().GetCast(ID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("get/credits/crew")]
        public async Task<IActionResult> GetFilmCrew([FromQuery] int ID)
        {
            try
            {
                var result = await new PeopleManager().GetCrew(ID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

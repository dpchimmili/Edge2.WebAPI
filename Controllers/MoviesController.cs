using Edge2.WebAPIs.Models;
using Edge2.WebAPIs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Edge2.WebAPIs.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private IMoviesService _MovieService;

        public MoviesController(IMoviesService movieService)
        {
            _MovieService = movieService;
        }

        [AllowAnonymous]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var movies = _MovieService.GetAll();
            return Ok(movies);
        }

        [AllowAnonymous]
        [HttpGet("GetSingle/{id}")]
        public IActionResult GetSingle(int id)
        {
            var movies = _MovieService.GetSingle(id);
            return Ok(movies);
        }

        [HttpPost("AddMovie")]
        public IActionResult AddMovie([FromBody] MovieModel model)
        {
            if (_MovieService.AddMovie(model))
            {
                return Ok();
            }
            else
                return BadRequest(new { message = "Movie could not be added." });

        }
        [HttpPut("UpdateMovie")]
        public IActionResult UpdateMovie([FromBody] MovieModel model)
        {
            if (_MovieService.UpdateMovie(model))
            {
                return Ok();
            }
            else
                return BadRequest(new { message = "Movie could not be updated." });

        }
        [HttpDelete("DeleteMovie/{id}")]
        public IActionResult DeleteMovie(int id)
        {
            if (_MovieService.DeleteMovie(id))
            {
                return Ok();
            }
            else
                return BadRequest(new { message = "Movie has not been deleted." });

        }
    }
}
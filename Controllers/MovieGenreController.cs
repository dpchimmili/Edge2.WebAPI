using Edge2.WebAPIs.Models;
using Edge2.WebAPIs.Services;
using Microsoft.AspNetCore.Mvc;

namespace Edge2.WebAPIs.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MovieGenreController : ControllerBase
    {
        private IMovieGenresService _MovieGenreService;
        public MovieGenreController(IMovieGenresService movieGenreService)
        {
            _MovieGenreService = movieGenreService;
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var moviesGenre = _MovieGenreService.GetAll();
            return Ok(moviesGenre);
        }

        [HttpGet("GetSingle/{id}")]
        public IActionResult GetSingle(int id)
        {
            var moviesGenre = _MovieGenreService.GetSingle(id);
            return Ok(moviesGenre);
        }

        [HttpPost("AddMovieGenre")]
        public IActionResult AddMovieGenre([FromBody] MovieGenreModel model)
        {
            if (_MovieGenreService.AddMovieGenre(model))
            {
                return Ok();
            }
            else
                return BadRequest(new { message = "Movie Genre could not be added." });

        }
        [HttpPost("UpdateMovieGenre")]
        public IActionResult UpdateMovieGenre([FromBody] MovieGenreModel model)
        {
            if (_MovieGenreService.UpdateMovieGenre(model))
            {
                return Ok();
            }
            else
                return BadRequest(new { message = "Movie Genre could not be updated." });

        }
        [HttpDelete("DeleteMovieGenre/{id}")]
        public IActionResult DeleteMovieGenre(int id)
        {
            if (_MovieGenreService.DeleteMovieGenre(id))
            {
                return Ok();
            }
            else
                return BadRequest(new { message = "Movie Genre has not been deleted." });

        }

    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NxGN_Server.Interfaces;
using NxGN_Server.Models;
using NxGN_Server.Models.Repositories;

namespace NxGN_Server.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieRepository _movieRepository;

        public MoviesController(ILogger<MoviesController> logger, IMovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

[HttpGet]
public async Task<IActionResult> GetAllMovies()
{
    var response = await _movieRepository.GetAllMoviesAsync();
    
    if (response.Success && response.Data.Any())
        return Ok(response);
    
    return NotFound(response);
}


        [HttpGet("{id}")]
public async Task<IActionResult> GetMovieById(int id)
{
    var movie = await _movieRepository.GetByIdAsync(id);
    if (movie != null)
        return Ok(new ServiceResponse<Movie> { Data = movie, Success = true, Message = "Movie fetched successfully." });
    return NotFound(new ServiceResponse<Movie> { Success = false, Message = "Movie not found." });
}
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] Movie movie)
        {
            var response = await _movieRepository.AddMovieAsync(movie);
            if (response.Success)
            return Ok(response);
            return BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movie movie)
        {
            if (id != movie.movieId)
            return BadRequest(new ServiceResponse<bool> { Success = false, Message = "Movie ID mismatch." });
    
            var response = await _movieRepository.UpdateMovieAsync(movie);
            if (response.Success)
            return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            ServiceResponse<bool> response = await _movieRepository.DeleteMovieByIdAsync(id);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
    }   
}
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMongoDBService _movieService;
        private readonly ITmdpService _tmdpService;
        private Movie movie;

        public MoviesController(IMongoDBService movieService, ITmdpService tmdpService)
        {
            _movieService = movieService;
            _tmdpService = tmdpService;
        }

        [HttpGet("{id}", Name = "GetDBMovie")]
        public async Task<ActionResult<Movie>> Get(int id)
        {
            movie = await _movieService.GetAsync(id);

            if (movie == null)
            {
                var tmdpmovie = await GetTmdpMovieAsync(id);
                //if (tmdpmovie == null)
                //{
                //    return NotFound();
                //}

                movie = tmdpmovie.Value;
                if (movie == null)
                {
                    return NotFound();
                }
                await _movieService.CreateAsync(movie);
            }

            return movie;
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> Create(Movie movie)
        {
            await _movieService.CreateAsync(movie);

            return CreatedAtRoute("GetMovie", new { id = movie.Id.ToString() }, movie);
        }

        [HttpGet("tmdp/{id}", Name = "GetTmdpMovie")]
        public async Task<ActionResult<Movie>> GetTmdpMovieAsync(int id)
        {
            var temp_movie = await _tmdpService.GetMovie(id);
            if (temp_movie == null)
            {
                return NotFound();
            }
            return temp_movie;
        }
    }
}

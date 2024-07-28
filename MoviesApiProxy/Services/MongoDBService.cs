using MongoDB.Driver;
using MoviesApiProxy.Data;
using MoviesApiProxy.Models;
using MoviesApiProxy.Middleware;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesApiProxy.Services
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongoCollection<Movie> _movies;

        public MongoDBService(IMovieContext context)
        {
            _movies = context.Movies;
        }

        public async Task<List<Movie>> GetAsync() =>
            await _movies.Find(movie => true).ToListAsync();

        public async Task<Movie> GetAsync(int id)
        {
            if (id <= 0)
            {
                throw new InvalidMovieIdException("The provided movie ID is invalid.");
            }
            var movie = await _movies.Find<Movie>(movie => movie.Id == id).FirstOrDefaultAsync();
            return movie;
        }

        public async Task CreateAsync(Movie movie) =>
            await _movies.InsertOneAsync(movie);
    }
}
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using MoviesApiProxy.Models;

namespace MoviesApiProxy.Data
{
    public class MovieContext : IMovieContext
    {
        private readonly IMongoDatabase _database;

        public MovieContext(IOptions<MovieDatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Movie> Movies => _database.GetCollection<Movie>(nameof(Movie));
    }
}

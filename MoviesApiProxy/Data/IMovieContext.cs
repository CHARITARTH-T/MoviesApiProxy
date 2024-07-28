using MongoDB.Driver;
using MoviesApiProxy.Models;

namespace MoviesApiProxy.Data
{
    public interface IMovieContext
    {
        IMongoCollection<Movie> Movies { get; }
    }
}

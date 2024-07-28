using System.Collections.Generic;
using System.Threading.Tasks;
using MoviesApiProxy.Models;

namespace MoviesApiProxy.Services
{
    public interface IMongoDBService
    {
        Task<List<Movie>> GetAsync();
        Task<Movie> GetAsync(int id);
        Task CreateAsync(Movie movie);
    }
}

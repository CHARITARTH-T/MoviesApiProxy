using MoviesApiProxy.Models;

namespace MoviesApiProxy.Services
{
    public interface ITmdpService
    {
        Task<Movie> GetMovie(int id);
    }
}

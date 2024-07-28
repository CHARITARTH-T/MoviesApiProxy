using MoviesApiProxy.Models;
using MoviesApiProxy.Middleware;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MoviesApiProxy.Services
{
    public class TmdpService : ITmdpService
    {
        private HttpClient _httpClient;
        public TmdpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Movie> GetMovie(int id)
        {
            if (id <= 0)
            {
                throw new InvalidMovieIdException("The provided movie ID is invalid.");
            }
            var api_key = "dc9631a2d144bc7c6ca44e1e6192528b";
            string APIURL = $"https://api.themoviedb.org/3/movie/{id}?api_key={api_key}";

            try
            {
                var movie = await _httpClient.GetFromJsonAsync<Movie>(APIURL);
                if (movie == null)
                {
                    throw new MovieNotFoundException("Movie not found.");
                }
                return movie;
            }
            catch (HttpRequestException)
            {
                throw new MovieNotFoundException("Movie not found.");
            }
        }
    }
}
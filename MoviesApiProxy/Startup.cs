using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MoviesApiProxy.Data;
using MoviesApiProxy.Middleware;
using MoviesApiProxy.Models;
using MoviesApiProxy.Services;
using StackExchange.Profiling;
using StackExchange.Profiling.Storage;

namespace MoviesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movies API", Version = "v1" });
            });

            // Configure MovieDatabaseSettings
            services.Configure<MovieDatabaseSettings>(
                Configuration.GetSection(nameof(MovieDatabaseSettings)));

            // Register MovieContext and MovieService using interfaces
            services.AddSingleton<IMovieContext, MovieContext>();
            services.AddScoped<IMongoDBService, MongoDBService>();
            services.AddScoped<ITmdpService, TmdpService>();

            services.AddHttpClient<ITmdpService, TmdpService>(client =>
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/movie");
            });
            //services.AddMiniProfiler(options =>
            //{
            //    options.RouteBasePath = "/profiler";
            //    // You can use other storage providers like Redis, SQL Server, etc.
            //    options.Storage = new MemoryCacheStorage(TimeSpan.FromMinutes(60));
            //}).AddEntityFramework();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movies API V1");
            });
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();
           
            app.UseRouting();
            //app.UseMiniProfiler();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

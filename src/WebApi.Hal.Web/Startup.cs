using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using WebApi.Hal.Web.Data;

namespace WebApi.Hal.Web
{
    public class Startup
    {
        #region Properties

        public IConfigurationRoot Configuration { get; set; }

        #endregion

        #region Public methods

        public Startup(IHostingEnvironment env,
            IApplicationEnvironment appEnv)
        {
            // Load all the configuration information from the "json" file & the environment variables.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["Data:DefaultConnection:ConnectionString"];
            services.AddLogging();
            services.AddMvc();
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<BeerDbContext>(opt => opt.UseSqlServer(connectionString));
            services.AddTransient<IRepository, BeerRepository>();
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "BeersRoute",
                    "beers/{id}",
                    new
                    {
                        controller = "Beer"
                    });
                routes.MapRoute(
                    "DefaultApi",
                    "{controller}/{id?}");
                routes.MapRoute(
                    "BreweryBeersRoute",
                    "breweries/{id}/beers",
                    new
                    {
                        controller = "BeersFromBrewery"
                    });
                routes.MapRoute(
                    "StyleBeersRoute",
                    "styles/{id}/beers",
                    new
                    {
                        controller = "BeersFromStyle"
                    });
            });
        }

        #endregion

        #region Public static methods

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);

        #endregion
    }
}

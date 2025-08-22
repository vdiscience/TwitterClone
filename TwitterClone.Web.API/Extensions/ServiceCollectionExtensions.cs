using TwitterClone.Web.API.Repositories;
using TwitterClone.Web.API.Services;

namespace TwitterClone.Web.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCityModule(this IServiceCollection services)
        {
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityService, CityService>();
            return services;
        }
    }
}
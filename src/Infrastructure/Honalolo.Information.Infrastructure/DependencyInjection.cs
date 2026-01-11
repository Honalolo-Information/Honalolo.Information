using Honalolo.Information.Domain.Entities.Interfaces;
using Honalolo.Information.Infrastructure.Persistance;
using Honalolo.Information.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Honalolo.Information.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Configure MariaDB Connection
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TouristInfoDbContext>(options =>
                options.UseMySql(connectionString,
                    ServerVersion.AutoDetect(connectionString)));

            // 2. Register Repositories
            services.AddScoped<IAttractionRepository, AttractionRepository>();
            // services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}

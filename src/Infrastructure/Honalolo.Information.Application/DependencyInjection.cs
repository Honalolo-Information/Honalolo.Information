using Microsoft.Extensions.DependencyInjection;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Application.Services;

namespace Honalolo.Information.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAttractionService, AttractionService>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}
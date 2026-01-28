using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Application.Services;
using Honalolo.Information.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Honalolo.Information.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            services.AddScoped<IAttractionService, AttractionService>();
            services.AddScoped<IDictionaryService, DictionaryService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPdfService, PdfService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
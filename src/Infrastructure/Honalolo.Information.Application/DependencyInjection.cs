using Microsoft.Extensions.DependencyInjection;
using Honalolo.Information.Application.Interfaces;
using Honalolo.Information.Application.Services;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAttractionService, AttractionService>();
        // services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
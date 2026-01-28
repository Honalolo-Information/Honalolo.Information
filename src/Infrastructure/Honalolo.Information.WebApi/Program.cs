using Honalolo.Information.Application;
using Honalolo.Information.Domain.Enums;
using Honalolo.Information.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Honalolo.Information.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWebApp",
                builder =>
                {
                    builder.WithOrigins("https://localhost:8000", "https://127.0.0.1:8000",
                           "http://localhost:8000", "http://127.0.0.1:8000")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            
            if (jwtSettings != null)
            {
                var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

                // Add authorization policies
                builder.Services.AddAuthorizationBuilder()
                    .AddPolicy("Administrator", policy =>
                        policy.RequireRole(UserRole.Administrator.ToString()))
                    .AddPolicy("Moderator", policy =>
                        policy.RequireRole(UserRole.Moderator.ToString()))
                    .AddPolicy("Partner", policy =>
                        policy.RequireRole(UserRole.Partner.ToString()))
                    .AddPolicy("RegisteredUser", policy =>
                        policy.RequireRole(UserRole.RegisteredUser.ToString()))
                    .AddPolicy("Guest", policy =>
                        policy.RequireRole(UserRole.Guest.ToString()));
            }

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowWebApp");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

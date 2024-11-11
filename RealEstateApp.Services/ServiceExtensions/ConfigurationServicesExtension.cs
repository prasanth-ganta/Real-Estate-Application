using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Database.Data;
using RealEstateApp.Database.Implementations;
using RealEstateApp.Database.Interfaces;
using RealEstateApp.Services.AutoMappers;
using RealEstateApp.Services.Implementations;
using RealEstateApp.Services.Interfaces;

namespace RealEstateApp.Services.ServiceExtensions;

public static class ConfigurationServicesExtension
{
    public static void ConfigurationServices(this IServiceCollection services,IConfiguration configuration){
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IUserService,UserService>();
        services.AddAutoMapper(typeof(MappingProfiles));
        services.AddDbContext<RealEstateDbContext>
        (
            options => options.UseSqlServer(configuration.GetConnectionString("DbString"))
        );
        services.AddAuthentication
        (
            options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
        ).AddJwtBearer
        (
            options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSetting:SecretKey"])),
            }
        );
        
        

        
    }
}


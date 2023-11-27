using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.OpenApi.Models;
using service.Services;

namespace api;

public static class ServiceCollectionExtensions
{
    public static void AddJwtService(this IServiceCollection services)
    {
        services.AddSingleton<JwtOptions>(service =>
        {
            var configuration = service.GetRequiredService<IConfiguration>();
            var options = configuration.GetRequiredSection("JWT").Get<JwtOptions>()!;
            // If address isn't set in the config then we are likely running in development mode.
            // We will use the address of the server as *issuer* for JWT.
            if (string.IsNullOrEmpty(options?.Issuer))
            {
                var server = service.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
                options.Issuer = addresses?.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(options?.Audience))
            {
                var server = service.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
                options.Audience = addresses?.FirstOrDefault();
            }

            return options;
        });
        services.AddSingleton<JwtService>();
    }

    public static void AddSwaggerGenWithBearerJWT(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT without Bearer prefix into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new string[] { }
                    }
                });
            }
        );
    }
}
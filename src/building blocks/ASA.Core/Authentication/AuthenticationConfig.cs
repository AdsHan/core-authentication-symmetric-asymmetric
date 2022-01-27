using ASA.Core.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace ASA.Core.Authentication;

public static class AuthenticationConfig
{
    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IGenerateSecurityKey, GenerateSecurityKey>();

        var generateSecurityKey = services.BuildServiceProvider().GetService<IGenerateSecurityKey>();

        var tokenSettings = configuration.GetSection("TokenSettings").Get<TokenSettings>();

        // Adiciona o JWT Bearer
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("symmetric", options => options.TokenValidationParameters = new TokenValidationParameters
            {
                SaveSigninToken = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidAudience = tokenSettings.Audience,
                ValidIssuer = tokenSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = generateSecurityKey.GetSymmetricKey(tokenSettings.SecretJWTKey)
            })
            .AddJwtBearer("asymmetric", options => options.TokenValidationParameters = new TokenValidationParameters
            {
                SaveSigninToken = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidAudience = tokenSettings.Audience,
                ValidIssuer = tokenSettings.Issuer,
                ValidateIssuerSigningKey = true,
                //IssuerSigningKey = generateSecurityKey.GetAsymmetricKeyPublic()
                IssuerSigningKeyResolver = (token, secutiryToken, kid, validationParameters) =>
                {
                    SecurityKey issuerSigningKey = generateSecurityKey.GetAsymmetricKeyPublic();
                    return new List<SecurityKey>() { issuerSigningKey };
                }
            });

        services.AddSwaggerGen(c =>
            {

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
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
                            }
                        },
                        new string[] { }
                    }
                });
            });

        return services;
    }

    public static IApplicationBuilder UseAuthenticationConfiguration(this IApplicationBuilder app)
    {
        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Core.Authorization;
using Core.DbSeeders;
using Core.Identity;
using Core.Services;
using DataLayer.Data;
using DataLayer.Infrastructure;
using DataLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace PaymentProject.Configuration;

public static class ServiceCollectionExtensions
{
    public static void Configure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddAppDbContext(configuration.GetConnectionString("Default"))
            .AddUserServices(configuration)
            .AddAuthorizationServices(configuration);
    }
    
    private static IServiceCollection AddUserServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IRoleChanger, RoleChanger>()
            .AddScoped<IUserCreator, UserCreator>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IPaymentService, PaymentService>()
            .AddScoped<IDataSeeder, DefaultRolesDbLoader>();
    }
    
    private static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings))).ConfigureOptions<JwtSettingsConfiguration>()
            .AddScoped<IAuthorizeService, AuthorizeService>()
            .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
    }

    
    public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var serviceProvider = services.BuildServiceProvider();
        var jwtOptions = serviceProvider.GetService<IOptions<JwtSettings>>();
        return services
            .AddIdentityAndConfigure(configuration)
            .ConfigureJwt(jwtOptions.Value);
    }
    
    private static IServiceCollection ConfigureJwt(this IServiceCollection services, JwtSettings jwtSettings)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(jwtSettings.Secret)),
                    ValidateIssuerSigningKey = true,
                };
            });

        return services;
    }
    
    private static IServiceCollection AddIdentityAndConfigure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IdentityOptions>(configuration.GetSection(nameof(IdentityOptions)));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
    
    public static IServiceCollection ConfigurationSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web Api", Version = "v1" });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
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
                    new List<string>()
                }
            });
        });
    }
}
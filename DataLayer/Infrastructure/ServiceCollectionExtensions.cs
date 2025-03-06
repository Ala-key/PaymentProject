using DataLayer.Data;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace DataLayer.Infrastructure;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Подключение БД контекста.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="connectionString"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppDbContext(this IServiceCollection services, string? connectionString) =>
        services
            .AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString, ConfigNpgsqloptionsBuilder);
            opt.EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });
    
    // <summary>
    /// Применение миграций, если они есть.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }

    private static void ConfigNpgsqloptionsBuilder(NpgsqlDbContextOptionsBuilder options)
    {
        options.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
        options.EnableRetryOnFailure
        (
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorCodesToAdd: null
        );
    }
}
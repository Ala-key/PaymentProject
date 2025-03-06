using Core.DbSeeders;

namespace PaymentProject.Configuration;

public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Заполнение ролей.
    /// </summary>
    /// <param name="app"></param>
    public static async Task FillRolesAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        await seeder.SeedAsync();
        
    }
}
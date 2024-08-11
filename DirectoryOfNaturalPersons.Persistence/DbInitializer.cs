using Microsoft.EntityFrameworkCore;

namespace DirectoryOfNaturalPersons.Persistence;

public class DbInitializer
{
    public async Task SeedAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
        try
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred during database seeding : {e.Message}");
        }
    }
}
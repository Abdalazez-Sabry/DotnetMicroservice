using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrebDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
        }

        private static void SeedData(AppDbContext? context, bool isProduction)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            if (isProduction)
            {
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"->> Couldn't run migrations, ${ex}");
                }
            }

            if (context.Platforms.Any())
            {
                Console.WriteLine("->> We already have data");
                return;
            }

            Console.WriteLine("->> Seeding Data");
            context.AddRange(
                new Platform() { Name = "Dotnet", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                new Platform() { Name = "Kebuernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
            );

            context.SaveChanges();
        }
    }
}

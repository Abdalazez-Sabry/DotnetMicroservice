using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrebDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }

        private static void SeedData(AppDbContext? context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

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

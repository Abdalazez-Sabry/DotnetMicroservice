using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public static class PrebDb
    {
        public static void PrebPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>() ?? throw new Exception("grpcClient is null, didn't add IPlatformDataClient to app services");

                var commandRepo = serviceScope.ServiceProvider.GetService<ICommandRepo>() ?? throw new Exception("ICommandRepo is null, didn't add ICommandRepo to app services");

                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData(commandRepo, platforms);

            }
        }
        private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("->> Seeding Data");
            foreach (var plat in platforms)
            {
                if (!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                }
            }

            repo.SaveChanges();
        }
    }
}
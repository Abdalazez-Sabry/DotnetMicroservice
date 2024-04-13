using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepo(AppDbContext context) : IPlatformRepo
    {
        private readonly AppDbContext _context = context;

        public void CreatePlatform(Platform plat)
        {
            ArgumentNullException.ThrowIfNull(plat);

            if (GetPlatformById(plat.Id) != default)
            {
                throw new InvalidOperationException("");
            }

            _context.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Platform? GetPlatformById(int id)
        {
            return _context.Platforms.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
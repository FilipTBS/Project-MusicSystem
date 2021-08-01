using MusicSystem.Data;
using MusicSystem.Services.Curators;
using System.Linq;

namespace MusicSystem.Services
{
    public class CuratorService : ICuratorService
    {
        private readonly MusicSystemDbContext data;

        public CuratorService(MusicSystemDbContext data)
            => this.data = data;

        public bool IsCurator(string userId)
            => this.data.Curators
                .Any(x => x.UserId == userId);

        public int IdByUser(string userId)
            => this.data.Curators
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault();
    }
}

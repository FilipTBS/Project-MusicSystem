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
            => this.data
                .Curators
                .Any(d => d.UserId == userId);

        public int IdByUser(string userId)
            => this.data
                .Curators
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();
    }
}

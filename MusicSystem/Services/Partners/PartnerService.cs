using MusicSystem.Data;
using System.Linq;

namespace MusicSystem.Services.Partners
{
    public class PartnerService : IPartnerService
    {
        private readonly MusicSystemDbContext data;

        public PartnerService(MusicSystemDbContext data)
            => this.data = data;

        public bool IsPartner(string userId)
            => this.data.Partners
                .Any(x => x.UserId == userId);

        public string IdByUser(string userId)
            => this.data.Partners
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault();
    }
}

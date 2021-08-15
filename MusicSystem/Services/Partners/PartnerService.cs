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

        public bool Exists(string businessEmail)
        => this.data.Partners
        .Any(x => x.BusinessEmail == businessEmail);

        public void Delete(string businessEmail)
        {
            var partner = this.data.Partners.Where(x => x.BusinessEmail == businessEmail).FirstOrDefault();

            if (partner != null)
            {
                var Id = partner.UserId;
                var user = this.data.Users.Where(x => x.Id == Id).FirstOrDefault();
                this.data.Partners.Remove(partner);
                this.data.Users.Remove(user);
                this.data.SaveChanges();
            }
        }
    }
}

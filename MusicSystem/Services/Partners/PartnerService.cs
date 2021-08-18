using MusicSystem.Data;
using MusicSystem.Data.Models;
using System.Linq;

namespace MusicSystem.Services.Partners
{
    public class PartnerService : IPartnerService
    {
        private readonly MusicSystemDbContext data;

        public PartnerService(MusicSystemDbContext data)
            => this.data = data;

        public string Add(string companyName, string businessEmail, 
            string website, string userId)
        {
            var partnerObject = new Partner
            {
                Name = companyName,
                BusinessEmail = businessEmail,
                Website = website,
                UserId = userId
            };

            this.data.Partners.Add(partnerObject);
            this.data.SaveChanges();

            return partnerObject.Id;
        }

        public bool IsPartner(string userId)
            => this.data.Partners
                .Any(x => x.UserId == userId);
        //.Partners.Any(d => d.UserId == userId);

        public bool CheckForSameEmail(string businessEmail)
        => this.data.Partners
        .Any(x => x.BusinessEmail == businessEmail);

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

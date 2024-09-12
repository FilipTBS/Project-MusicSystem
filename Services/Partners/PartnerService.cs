using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MusicSystem.Services.Partners
{
    public class PartnerService : IPartnerService
    {
        private readonly MusicSystemDbContext data;

        public PartnerService(MusicSystemDbContext data)
            => this.data = data;

        public async Task<string> AddAsync(string companyName, string businessEmail, 
            string website, string userId)
        {
            var partnerObject = new Partner
            {
                Name = companyName,
                BusinessEmail = businessEmail,
                Website = website,
                UserId = userId
            };

            await this.data.Partners.AddAsync(partnerObject);
            await this.data.SaveChangesAsync();

            return partnerObject.Id;
        }

        public async Task<bool> IsPartnerAsync(string userId)
        {
                return await this.data.Partners
                .AnyAsync(x => x.UserId == userId);
                //.Partners.Any(d => d.UserId == userId);
        }

        public async Task<bool> CheckForSameEmailAsync(string businessEmail)
        {
            return await this.data.Partners
                .AnyAsync(x => x.BusinessEmail == businessEmail);
        }

        public async Task<string> IdByUserAsync(string userId)
        {
            return await this.data.Partners
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        } 
        
        public async Task<bool> ExistsAsync(string businessEmail)
        {
           return await this.data.Partners
                  .AnyAsync(x => x.BusinessEmail == businessEmail);
        }

        public async Task DeleteAsync(string businessEmail)
        {
            var partner = await this.data.Partners.Where(x => x.BusinessEmail == businessEmail).FirstOrDefaultAsync();

            if (partner != null)
            {
                var Id = partner.UserId;
                var user = await this.data.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
                this.data.Partners.Remove(partner);
                this.data.Users.Remove(user);
                await this.data.SaveChangesAsync();
            }
        }
    }
}

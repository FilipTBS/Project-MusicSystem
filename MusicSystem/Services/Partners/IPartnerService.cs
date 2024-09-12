using System.Threading.Tasks;

namespace MusicSystem.Services.Partners
{
    public interface IPartnerService
    {
        public Task<string> AddAsync(string companyName, string businessEmail,
            string website, string userId);

        public Task<bool> IsPartnerAsync(string userId);

        public Task<string> IdByUserAsync(string userId);

        public Task<bool> ExistsAsync(string businessEmail);

        public Task DeleteAsync(string businessEmail);

        public Task<bool> CheckForSameEmailAsync(string businessEmail);
    }
}

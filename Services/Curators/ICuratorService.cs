using System.Threading.Tasks;

namespace MusicSystem.Services.Curators
{
    public interface ICuratorService
    {
        public Task<string> AddAsync(string nickname, string email, string userId);

        public Task<bool> IsCuratorAsync(string userId);

        public Task<string> IdByUserAsync(string userId);

        public Task<bool> ExistsAsync(string email);

        public Task DeleteAsync(string email);

        public Task<bool> CheckForSameEmailAsync(string email);
    }
}
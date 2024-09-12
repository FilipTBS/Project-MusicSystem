using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Services.Curators;
using System.Linq;
using System.Threading.Tasks;

namespace MusicSystem.Services
{
    public class CuratorService : ICuratorService
    {
        private readonly MusicSystemDbContext data;

        public CuratorService(MusicSystemDbContext data)
            => this.data = data;

        public async Task<string> AddAsync(string nickname, string email, string userId)
        {
            var curatorObject = new Data.Curator
            {
                Nickname = nickname,
                Email = email,
                UserId = userId
            };

            await this.data.Curators.AddAsync(curatorObject);
            await this.data.SaveChangesAsync();

            return curatorObject.Id;
        }

        public async Task<bool> IsCuratorAsync(string userId)
        {
            return this.data.Curators
                .Any(x => x.UserId == userId);
                //.Curators.Any(d => d.UserId == userId);
        }

        public async Task<bool> CheckForSameEmailAsync(string email)
        {
            return await this.data.Curators
                     .AnyAsync(x => x.Email == email);
        }

        public async Task<string> IdByUserAsync(string userId)
        {
            return await this.data.Curators
            .Where(x => x.UserId == userId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await this.data.Curators
                    .AnyAsync(x => x.Email == email);
        }

        public async Task DeleteAsync(string email)
        {
            var curator = await this.data.Curators.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (curator != null)
            {
                var Id = curator.UserId;
                var user = await this.data.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
                this.data.Curators.Remove(curator);
                this.data.Users.Remove(user);
                await this.data.SaveChangesAsync();
            }
        }
    }
}
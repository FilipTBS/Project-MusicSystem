using MusicSystem.Data;
using MusicSystem.Services.Curators;
using System.Linq;
using static MusicSystem.Data.Constants;

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

        public string IdByUser(string userId)
            => this.data.Curators
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefault();

        public bool Exists(string email)
        => this.data.Curators
        .Any(x => x.Email == email);

        public void Delete(string email)
        {
            var curator = this.data.Curators.Where(x => x.Email == email).FirstOrDefault();

            if (curator != null)
            {
                var Id = curator.UserId;
                var user = this.data.Users.Where(x => x.Id == Id).FirstOrDefault();
                this.data.Curators.Remove(curator);
                this.data.Users.Remove(user);
                this.data.SaveChanges();
            }
        }
    }
}
using System.Linq;
using MusicSystem.Data;

namespace MusicSystem.Services.Statistics
{
    public class StatisticsService : IStatisticsService
    {
        private readonly MusicSystemDbContext data;

        public StatisticsService(MusicSystemDbContext data)
            => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalSongs = this.data.Songs.Count(x => x.IsApproved);
            var totalUsers = this.data.Users.Count();
            var totalArtists = this.data.Artists.Count();

            return new StatisticsServiceModel
            {
                TotalSongs = totalSongs,
                TotalUsers = totalUsers,
                TotalArtists = totalArtists
            };
        }
    }
}

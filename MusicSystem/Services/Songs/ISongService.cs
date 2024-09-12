using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicSystem.Services.Songs
{
    public interface ISongService
    {
        Task<SongQueryServiceModel> AllAsync(string artist = null,
                                 string searchTerm = null,
                                 int currentPage = 1,
                                 int songsPerPage = 10,
                                 bool approvedOnly = true);

        Task<SongLyricsServiceModel> GetLyricsAsync(string songId);
        Task<SongInfoServiceModel> GetSongInfoAsync(string songId);

        Task<IEnumerable<ExampleSongServiceModel>> ExampleAsync();

        Task<string> AddAsync(
            string title,
            string artistId,
            string genre,
            string lyrics,
            string songUrl,
            string curatorId, 
            bool isApproved);

        Task<bool> EditAsync(
            string songId,
            string title,
            string artistId,
            string lyrics,
            string songUrl,
            string genre,
            bool isApproved);

        public Task DeleteAsync(string songId);

        public Task ChangeVisilityAsync(string songId);

        Task<ICollection<SongServiceModel>> ByUserAsync(string userId);

        Task<bool> IsByCuratorAsync(string songId, string curatorId);

        Task<ICollection<SongArtistServiceModel>> AllArtistsAsync();

        Task<bool> ArtistExistsAsync(string artistId);
    }
}

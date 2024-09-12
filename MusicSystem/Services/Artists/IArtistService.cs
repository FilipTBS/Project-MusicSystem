using MusicSystem.Data.Models;
using MusicSystem.Models.Artists;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicSystem.Services
{
    public interface IArtistService
    {
        Task<ArtistQueryServiceModel> CatalogueAsync(string artist,
                                         string searchTerm,
                                         int currentPage,
                                         int artistsPerPage);

        public Task<ArtistSongsViewModel> GetArtistSongsAsync(string id);

        public Task<bool> ExistsAsync(string name);

        public Task<string> AddAsync(string name, string genre, ICollection<Song> songs);

        public Task DeleteAsync(string name);

        public Task<Artist> FindArtistAsync(string name);
    }
}

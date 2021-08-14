using MusicSystem.Data.Models;
using MusicSystem.Models.Artists;
using System.Collections.Generic;

namespace MusicSystem.Services
{
    public interface IArtistService
    {
        ArtistQueryServiceModel Catalogue(string artist,
                                 string searchTerm,
                                 int currentPage,
                                 int artistsPerPage);

        public ArtistSongsViewModel GetArtistSongs(string id);

        public bool Exists(string name);

        public string Add(string name, string genre, ICollection<Song> songs);

        public void Delete(string name);

        public Artist FindArtist(string name);
    }
}

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
    }
}

using MusicSystem.Models.Songs;
using System.Collections.Generic;

namespace MusicSystem.Services.Songs
{
    public interface ISongService
    {
        SongQueryServiceModel All(string artist,
                                 string searchTerm,
                                 SongSorting sorting,
                                 int currentPage,
                                 int songsPerPage);

        string Create(
            string title,
            string artistId,
            string genre,
            string lyrics,
            string language,
            string songUrl,
            int curatorId);

        /*bool Edit(
            int carId,
            string brand,
            string model,
            string description,
            string imageUrl,
            int year,
            int categoryId);*/

        //IEnumerable<SongServiceModel> ByUser(string userId);

        bool IsByCurator(string songId, int curatorId);

        IEnumerable<string> AllArtists();

        bool ArtistExists(string artistId);
    }
}

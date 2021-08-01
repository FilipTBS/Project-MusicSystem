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

        SongLyricsServiceModel Lyrics(string songId);
        SongInfoServiceModel GetSongInfo(string songId);

        string Create(
            string title,
            string artistId,
            string genre,
            string lyrics,
            string songUrl,
            int curatorId);

        bool Edit(
            string songId,
            string title,
            string artistId,
            string lyrics,
            string songUrl,
            string genre);

        IEnumerable<SongServiceModel> ByUser(string userId);

        bool IsByCurator(string songId, int curatorId);

        IEnumerable<SongArtistModel> AllArtists();

        bool ArtistExists(string artistId);
    }
}

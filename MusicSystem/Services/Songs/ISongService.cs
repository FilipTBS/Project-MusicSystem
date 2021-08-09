using MusicSystem.Models.Songs;
using System.Collections.Generic;

namespace MusicSystem.Services.Songs
{
    public interface ISongService
    {
        SongQueryServiceModel All(string artist = null,
                                 string searchTerm = null,
                                 SongSorting sorting = SongSorting.DateCreated,
                                 int currentPage = 1,
                                 int songsPerPage = 10,
                                 bool approvedOnly = true);

        SongLyricsServiceModel GetLyrics(string songId);
        SongInfoServiceModel GetSongInfo(string songId);

        IEnumerable<LatestSongServiceModel> Latest();

        string Add(
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
            string genre,
            bool isApproved);

        public void ChangeVisility(string songId);

        IEnumerable<SongServiceModel> ByUser(string userId);

        bool IsByCurator(string songId, int curatorId);

        IEnumerable<SongArtistServiceModel> AllArtists();

        bool ArtistExists(string artistId);
    }
}

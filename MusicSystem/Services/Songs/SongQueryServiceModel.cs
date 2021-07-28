using System.Collections.Generic;

namespace MusicSystem.Services.Songs
{
    public class SongQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int SongsPerPage { get; init; }

        public int TotalSongs { get; init; }

        public IEnumerable<SongServiceModel> Songs { get; init; }
    }
}

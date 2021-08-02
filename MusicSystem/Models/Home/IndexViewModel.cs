using MusicSystem.Services.Songs;
using System.Collections.Generic;

namespace MusicSystem.Models.Home
{
    public class IndexViewModel
    { 
        public int TotalSongs { get; init; }

        public int TotalUsers { get; init; }

        public IList<LatestSongServiceModel> Songs { get; init; }

    }
}

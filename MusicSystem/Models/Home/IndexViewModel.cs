using System.Collections.Generic;

namespace MusicSystem.Models.Home
{
    public class IndexViewModel
    { 
        public int TotalSongs { get; init; }

        public int TotalUsers { get; init; }

        public List<SongIndexViewModel> Songs { get; init; }

    }
}

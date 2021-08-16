using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Models.Songs
{
    public class SongsQueryModel
    {
        public const int SongsPerPage = 10;
        public int CurrentPage { get; init; } = 1;

        public string Artist { get; set; }

        public IEnumerable<string> Artists { get; set; }

        [Display(Name = "Search by title (song name):")]
        public string SearchTerm { get; set; }

        public IEnumerable<SongListingViewModel> Songs { get; set; }

        public int TotalSongs { get; set; }
    }
}

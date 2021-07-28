using MusicSystem.Services.Songs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants;

namespace MusicSystem.Models.Songs
{
    public class AllSongsQueryModel
    {
        public const int SongsPerPage = 10;
        public int CurrentPage { get; init; } = 1;

        public string Artist { get; init; }

        public int TotalSongs { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }

        public SongSorting Sorting { get; init; }

        public IEnumerable<string> Artists { get; set; }

        public IEnumerable<SongServiceModel> Songs { get; set; }
    }
}

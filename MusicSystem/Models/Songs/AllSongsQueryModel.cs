using MusicSystem.Services.Songs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Models.Songs
{
    public class AllSongsQueryModel
    {
        public const int SongsPerPage = 10;
        public int CurrentPage { get; init; } = 1;

        public string Artist { get; init; }

        public int TotalSongs { get; set; }

        [Display(Name = "Search by Artist name")]
        public string SearchTerm { get; init; }

        public IEnumerable<SongArtistServiceModel> Artists { get; set; }

        public IEnumerable<SongServiceModel> Songs { get; set; }
    }
}

using MusicSystem.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Models.Artists
{
    public class CatalogueArtistsQueryModel
    {
        public const int ArtistsPerPage = 10;
        public int CurrentPage { get; init; } = 1;

        public string Artist { get; init; }

        public int TotalArtists { get; set; }

        [Display(Name = "Search by Artist name")]
        public string SearchTerm { get; init; }

        public IEnumerable<ArtistSongsViewModel> Songs { get; set; }

        public IEnumerable<ArtistServiceModel> Artists { get; set; }

    }
}

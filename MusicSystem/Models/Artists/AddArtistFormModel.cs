using MusicSystem.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Artist;

namespace MusicSystem.Models.Artists
{
    public class AddArtistFormModel
    {
        [Required, StringLength(ArtistNameMaxValue, MinimumLength = ArtistNameMinValue)]
        public string Name { get; set; }

        [Required, MaxLength(ArtistGenreMaxValue)]
        public string Genre { get; set; }
        public ICollection<Song> Songs { get; set; }
            = new List<Song>();
    }
}

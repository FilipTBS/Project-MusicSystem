using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Artist;

namespace MusicSystem.Models.Artists
{
    public class DeleteArtistFormModel
    {
        [Required, StringLength(ArtistNameMaxValue, MinimumLength = ArtistNameMinValue)]
        public string Name { get; set; }
    }  
}

using MusicSystem.Services.Songs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Song;

namespace MusicSystem.Models.Songs
{
    public class SongFormModel
    {
        [Required, StringLength(SongTitleMaxValue, MinimumLength = SongTitleMinValue)]
        public string Title { get; init; }

        [Display(Name = "Artist")]
        public string ArtistId { get; set; }

        public ICollection<SongArtistServiceModel> Artists { get; set; }

        [Required, StringLength(SongGenreMaxValue, MinimumLength = SongGenreMinValue)]
        public string Genre { get; init; }

        [Required, StringLength(SongLyricsMaxValue, MinimumLength = SongLyricsMinValue)]
        public string Lyrics { get; init; }

        [Required, Url, Display(Name = "Song URL")]
        public string SongUrl { get; init; }
    }
}

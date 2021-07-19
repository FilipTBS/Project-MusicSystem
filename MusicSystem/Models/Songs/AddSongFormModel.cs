using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants;

namespace MusicSystem.Models.Songs
{
    public class AddSongFormModel
    {
        [Required, StringLength(SongTitleMaxValue, MinimumLength = SongTitleMinValue)]
        public string Title { get; init; }

        [Display(Name = "Artist")]
        public string ArtistId { get; init; }

        public IEnumerable<SongArtistViewModel> Artists { get; set; }

        [Required, StringLength(SongGenreMaxValue, MinimumLength = SongGenreMinValue)]
        public string Genre { get; init; }

        [Required, MinLength(SongLyricsMinValue)]
        public string Lyrics { get; init; }

        [Required, StringLength(SongLanguageMaxValue, MinimumLength = SongLanguageMinValue)]
        public string Language { get; init; }

        [Required, Url, Display(Name = "Song URL")]
        public string SongUrl { get; init; }
    }
}

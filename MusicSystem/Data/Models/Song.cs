using System;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants;

namespace MusicSystem.Data.Models
{
    public class Song
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(SongTitleMaxValue)]
        public string Title { get; set; }

        public Artist Artist { get; set; }

        public string ArtistId { get; set; }


        [Required, MaxLength(SongGenreMaxValue)]
        public string Genre { get; set; }

        public string Lyrics { get; set; }

        public bool CuratorVerified { get; set; }

        [Required, MaxLength(SongLanguageMaxValue)]
        public string Language { get; set; }

        [Required]
        public string SongUrl { get; set; }

        public int Likes { get; set; }
    }
}

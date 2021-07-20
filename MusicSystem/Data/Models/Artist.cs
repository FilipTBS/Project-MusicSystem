using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Artist;

namespace MusicSystem.Data.Models
{
    public class Artist
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(ArtistNameMaxValue)]
        public string Name { get; set; }

        [Required, MaxLength(ArtistGenreMaxValue)]
        public string Genre { get; set; }
        public ICollection<Song> Songs { get; set; }
            = new List<Song>();

        public int SongsCount => this.Songs.Count;
    }
}
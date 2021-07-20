﻿using System;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Song;

namespace MusicSystem.Data.Models
{
    public class Song
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, MaxLength(SongTitleMaxValue)]
        public string Title { get; set; }

        public Artist Artist { get; init; }

        public string ArtistId { get; set; }


        [Required, MaxLength(SongGenreMaxValue)]
        public string Genre { get; set; }

        [Required]
        public string Lyrics { get; set; }

        public bool CuratorVerified { get; set; }

        [Required, MaxLength(SongLanguageMaxValue)]
        public string Language { get; set; }

        [Required]
        public string SongUrl { get; set; }

        public int Likes { get; set; }

        public string CuratorId { get; init; }

        public Curator Curator { get; init; }
    }
}
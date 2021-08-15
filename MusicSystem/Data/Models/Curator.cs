using MusicSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Curator;

namespace MusicSystem.Data
{
    public class Curator
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required, MaxLength(CuratorNicknameMaxValue)]
        public string Nickname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<Song> Songs { get; init; }
        = new List<Song>();
    }
}

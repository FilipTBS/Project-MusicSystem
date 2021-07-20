using Microsoft.AspNetCore.Identity;
using MusicSystem.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Curator;

namespace MusicSystem.Data
{
    public class Curator
    {
        public int Id { get; init; }

        [Required, MaxLength(CuratorNameMaxValue)]
        public string Name { get; set; }

        [Required, MaxLength(CuratorPhoneNumberMaxValue)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public IEnumerable<Song> Songs { get; init; }
        = new List<Song>();
    }
}

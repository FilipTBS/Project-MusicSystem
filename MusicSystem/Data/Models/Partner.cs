using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Data.Models
{
    public class Partner : Company
    {
        [Required, EmailAddress]
        public string BusinessEmail { get; set; }

        [Required, Url]
        public string Website { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static MusicSystem.Data.Constants.User;

namespace MusicSystem.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }
    }
}

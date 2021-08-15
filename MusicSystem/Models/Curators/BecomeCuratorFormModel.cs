using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Curator;

namespace MusicSystem.Models.Curators
{
    public class BecomeCuratorFormModel
    {
        [Required]
        [StringLength(CuratorNicknameMaxValue, MinimumLength = CuratorNicknameMinValue)]
        public string Nickname { get; set; }

        [Required]
        [Display(Name = "Email Address")]      
        [EmailAddress]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Curator;

namespace MusicSystem.Models.Curators
{
    public class BecomeCuratorFormModel
    {
        [Required]
        [StringLength(CuratorNameMaxValue, MinimumLength = CuratorNameMinValue)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]      
        [StringLength(CuratorPhoneNumberMaxValue, MinimumLength = CuratorPhoneNumberMinValue)]
        public string PhoneNumber { get; set; }
    }
}

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
        //[RegularExpression(@"^([\+]?(?:00)?[0-9]{1,3}[\s.-]?[0-9]{1,12})([\s.-]?[0-9]{1,4}?)$")]
        [StringLength(CuratorPhoneNumberMaxValue, MinimumLength = CuratorPhoneNumberMinValue)]
        public string PhoneNumber { get; set; }
    }
}

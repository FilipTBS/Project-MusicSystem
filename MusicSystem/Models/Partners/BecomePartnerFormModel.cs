using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Partner;

namespace MusicSystem.Models.Partners
{
    public class BecomePartnerFormModel
    {
        [Required]
        [StringLength(CompanyNameMaxValue, MinimumLength = CompanyNameMinValue)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Business Email")]
        [EmailAddress]
        public string BusinessEmail { get; set; }

        [Required]
        [Url]
        public string Website { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Company;

namespace MusicSystem.Models.Partners
{
    public class BecomePartnerFormModel
    {
        [Required]
        [StringLength(CompanyNameMaxValue, MinimumLength = CompanyNameMinValue)]
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "Business Email")]
        [EmailAddress]
        public string BusinessEmail { get; set; }

        [Required, Url]
        public string Website { get; set; }
    }
}

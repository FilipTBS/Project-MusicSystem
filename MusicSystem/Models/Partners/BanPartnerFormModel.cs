using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Models.Partners
{
    public class BanPartnerFormModel
    {
        [Required, EmailAddress]
        public string BusinessEmail { get; set; }
    }
}

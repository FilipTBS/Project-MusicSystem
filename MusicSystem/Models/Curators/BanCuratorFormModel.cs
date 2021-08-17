using System.ComponentModel.DataAnnotations;

namespace MusicSystem.Models.Curators
{
    public class BanCuratorFormModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}

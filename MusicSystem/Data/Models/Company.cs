using System;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Company;

namespace MusicSystem.Data.Models
{
    public class Company
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required, MaxLength(CompanyNameMaxValue)]
        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Feedback;

namespace MusicSystem.Data.Models
{
    public class Feedback
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string UserId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required, MaxLength(FeedbackSuggestionMaxValue)]
        public string Suggestion { get; set; }

    }
}

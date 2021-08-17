using System.ComponentModel.DataAnnotations;
using static MusicSystem.Data.Constants.Feedback;

namespace MusicSystem.Models.Feedbacks
{
    public class AddFeedbackModel
    {
        [Required]
        [Display(Name = "Score (rate)")]
        public int Score { get; set; }

        [StringLength(FeedbackSuggestionMaxValue, MinimumLength = FeedbackSuggestionMinValue)]
        public string Suggestion { get; set; }
    }
}

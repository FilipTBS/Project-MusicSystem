using System.Collections.Generic;

namespace MusicSystem.Services.Feedbacks
{
    public class FeedbackQueryServiceModel
    {
        public int TotalFeedbacks { get; set; }

        public ICollection<FeedbackServiceModel> Feedbacks { get; set; }
    }
}

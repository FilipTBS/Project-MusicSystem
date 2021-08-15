using MusicSystem.Data;
using MusicSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace MusicSystem.Services.Feedbacks
{
    public class FeedbackService : IFeedbackService
    {
        private readonly MusicSystemDbContext data;

        public FeedbackService(MusicSystemDbContext data)
            => this.data = data;

        public string AddFeedback(string userId,
           int score, string suggestion)
        {
            var feedbackData = new Feedback
            {
                UserId = userId,
                Score = score,
                Suggestion = suggestion,
            };

            this.data.Feedbacks.Add(feedbackData);
            this.data.SaveChanges();

            return feedbackData.Id;
        }
        public FeedbackQueryServiceModel All()
        {
            var feedbackQuery = this.data.Feedbacks.AsQueryable();
            var totalfeedbacks = feedbackQuery.Count();

            var feedbacks = GetFeedbacks(feedbackQuery);

            return new FeedbackQueryServiceModel
            {
                TotalFeedbacks = totalfeedbacks,
                Feedbacks = feedbacks
            };
        }

        public bool UserHasGivenFeedback(string id)
        {
            var userGaveFeedback = this.data.Feedbacks.Where(x => x.UserId == id);
            if (userGaveFeedback != null)
            {
                return true;
            }
            return false;
        }

        private static ICollection<FeedbackServiceModel> GetFeedbacks(IQueryable<Feedback> feedbackQuery)
        => feedbackQuery.Select(x => new FeedbackServiceModel
        {
           UserId = x.UserId,
           Score = x.Score,
           Suggestion = x.Suggestion
        }).ToList();

    }
}

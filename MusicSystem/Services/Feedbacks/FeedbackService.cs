using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicSystem.Services.Feedbacks
{
    public class FeedbackService : IFeedbackService
    {
        private readonly MusicSystemDbContext data;

        public FeedbackService(MusicSystemDbContext data)
            => this.data = data;

        public async Task<string> AddFeedbackAsync(string userId,
           int score, string suggestion)
        {
            var feedbackData = new Feedback
            {
                UserId = userId,
                Score = score,
                Suggestion = suggestion,
            };

            await this.data.Feedbacks.AddAsync(feedbackData);
            await this.data.SaveChangesAsync();

            return feedbackData.Id;
        }
        public async Task<FeedbackQueryServiceModel> AllAsync()
        {
            var feedbackQuery =  this.data.Feedbacks.AsQueryable();
            var totalfeedbacks = await feedbackQuery.CountAsync();

            var feedbacks = await GetFeedbacksAsync(feedbackQuery);

            return new FeedbackQueryServiceModel
            {
                TotalFeedbacks = totalfeedbacks,
                Feedbacks = feedbacks
            };
        }

        public async Task<bool> UserHasGivenFeedbackAsync(string id)
        {
            var userGaveFeedback = await this.data.Feedbacks.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (userGaveFeedback != null)
            {
                return true;
            }
            return false;
        }

        private static async Task<ICollection<FeedbackServiceModel>> GetFeedbacksAsync(IQueryable<Feedback> feedbackQuery)
        {
            return await feedbackQuery.Select(x => new FeedbackServiceModel
            {
                UserId = x.UserId,
                Score = x.Score,
                Suggestion = x.Suggestion
            }).ToListAsync();
        }

    }
}

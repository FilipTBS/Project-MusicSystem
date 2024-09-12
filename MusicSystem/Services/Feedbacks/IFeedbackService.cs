using System.Threading.Tasks;

namespace MusicSystem.Services.Feedbacks
{
    public interface IFeedbackService
    {
        public Task<string> AddFeedbackAsync(string userId,
           int score, string suggestion);

        public Task<FeedbackQueryServiceModel> AllAsync();

        public Task<bool> UserHasGivenFeedbackAsync(string id);

    }
}

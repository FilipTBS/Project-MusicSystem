namespace MusicSystem.Services.Feedbacks
{
    public interface IFeedbackService
    {
        public string AddFeedback(string userId,
           int score, string suggestion);

        public FeedbackQueryServiceModel All();

        public bool UserHasGivenFeedback(string id);
    }
}

namespace MusicSystem.Services
{
    public class FeedbackServiceModel
    {
        public string UserId { get; set; }

        public int Score { get; set; }

        public string Suggestion { get; set; }
    }
}
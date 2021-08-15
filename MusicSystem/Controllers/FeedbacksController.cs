using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure;
using MusicSystem.Models.Feedbacks;
using MusicSystem.Services.Feedbacks;
using static MusicSystem.Constants;

namespace MusicSystem.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly IFeedbackService feedbacks;

        public FeedbacksController(IFeedbackService feedbacks)
        => this.feedbacks = feedbacks;

        [Authorize]
        public IActionResult Index() => View();

        [Authorize]
        public IActionResult Add()
        {
            return View(new AddFeedbackModel { });
        }

        public IActionResult All([FromQuery] FeedbackQueryServiceModel query)
        {
            var queryResult = this.feedbacks.All();

            query.TotalFeedbacks = queryResult.TotalFeedbacks;
            query.Feedbacks = queryResult.Feedbacks;

            return View(query);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(AddFeedbackModel feedback)
        {
            var userId = this.User.GetId();

            if (this.feedbacks.UserHasGivenFeedback(userId))
            {
                TempData[GlobalMessageKey] = "You already provided your feedback";

                return RedirectToAction(nameof(All));
            }
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            this.feedbacks.AddFeedback(userId,
               feedback.Score,
               feedback.Suggestion);

            TempData[GlobalMessageKey] = "You added a feedback";

            return RedirectToAction(nameof(All));

        }
    }
}

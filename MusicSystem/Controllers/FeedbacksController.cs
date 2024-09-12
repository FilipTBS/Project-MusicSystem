using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Feedbacks;
using MusicSystem.Services.Feedbacks;
using System.Threading.Tasks;
using static Constants;

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

        public async Task<IActionResult> All([FromQuery] FeedbackQueryServiceModel query)
        {
            var queryResult = await this.feedbacks.AllAsync();

            query.TotalFeedbacks = queryResult.TotalFeedbacks;
            query.Feedbacks = queryResult.Feedbacks;

            return View(query);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddFeedbackModel feedback)
        {
            var userId = this.User.GetId();

            if (await this.feedbacks.UserHasGivenFeedbackAsync(userId))
            {
                TempData[GlobalMessageKey] = "You already provided your feedback";

                return RedirectToAction(nameof(All));
            }
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }

            await this.feedbacks.AddFeedbackAsync(userId,
               feedback.Score,
               feedback.Suggestion);

            TempData[GlobalMessageKey] = "You gave your Feedback";

            return RedirectToAction(nameof(All));

        }
    }
}

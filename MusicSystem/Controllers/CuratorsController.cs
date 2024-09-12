using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Curators;
using MusicSystem.Services.Curators;
using System.Threading.Tasks;
using static Constants;

namespace MusicSystem.Controllers
{
    public class CuratorsController : Controller
    {
        private readonly ICuratorService curators;

        public CuratorsController(ICuratorService curators)
        => this.curators = curators;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Become(BecomeCuratorFormModel curator)
        {
            var userId = this.User.GetId();

            var userIsAlreadyCurator = this.curators.IsCuratorAsync(userId);

            var email = curator.Email;
            var sameEmailAlreadyRegistered = this.curators.CheckForSameEmailAsync(email);

            if (await sameEmailAlreadyRegistered)
            {
                TempData[GlobalMessageKey] = "That email is used by another Curator!";
                return View(curator);
            }

            if (await userIsAlreadyCurator)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(curator);
            }

            await this.curators.AddAsync(curator.Nickname, curator.Email, userId);

            TempData[GlobalMessageKey] = "Thank you for becoming a Curator!";

            return RedirectToAction("All", "Songs");
        }

    }
}
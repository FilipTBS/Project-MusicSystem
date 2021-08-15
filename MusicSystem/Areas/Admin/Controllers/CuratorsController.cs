using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models.Curators;
using MusicSystem.Services.Curators;
using static MusicSystem.Constants;

namespace MusicSystem.Areas.Admin.Controllers
{
    public class CuratorsController : AdminController
    {
        private readonly ICuratorService curators;

        public CuratorsController(ICuratorService curators)
        => this.curators = curators;

        public IActionResult Ban()
        {
            return View(new BanCuratorFormModel { });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Ban(BanCuratorFormModel curator)
        {
            if (!this.curators.Exists(curator.Email))
            {
                this.ModelState.AddModelError(nameof(curator.Email), "Curator with that Email doesn't exist");
            }

            if (!ModelState.IsValid)
            {
                return View(curator);
            }
            var curatorEmail = curator.Email;
            this.curators.Delete(curatorEmail);

            TempData[GlobalMessageKey] = "You banned a Curator";

            return RedirectToAction("Manage", "Songs");
        }
    }
}

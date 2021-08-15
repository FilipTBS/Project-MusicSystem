using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Infrastructure;
using MusicSystem.Models.Curators;
using System.Linq;
using static MusicSystem.Constants;

namespace MusicSystem.Controllers
{
    public class CuratorsController : Controller
    {
        private readonly MusicSystemDbContext data;

        public CuratorsController(MusicSystemDbContext data)
        => this.data = data;

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeCuratorFormModel curator)
        {
            var userId = this.User.GetId();

            var userIsAlreadyCurator = this.data
                .Curators.Any(d => d.UserId == userId);

            if (userIsAlreadyCurator)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(curator);
            }

            var curatorObject = new Curator
            {
                Nickname = curator.Nickname,
                Email = curator.Email,
                UserId = userId
            };

            this.data.Curators.Add(curatorObject);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Thank you for becoming a Curator!";

            return RedirectToAction("All", "Songs");
        }

    }
}

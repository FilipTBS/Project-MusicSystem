using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models.Artists;
using MusicSystem.Services;
using static Constants;

namespace MusicSystem.Areas.Admin.Controllers
{
    public class ArtistsController : AdminController
    {
        private readonly IArtistService artists;

        public ArtistsController(IArtistService artists)
        => this.artists = artists;

        public IActionResult ArtistSongs(string id)
        {
            var artist = artists.GetArtistSongs(id);

            return View(artist);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View(new AddArtistFormModel { });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(AddArtistFormModel artist)
        {
            if (this.artists.Exists(artist.Name))
            {
                this.ModelState.AddModelError(nameof(artist.Name), "Artist with this name already exists");
            }

            if (!ModelState.IsValid)
            {
                return View(artist);
            }

            this.artists.Add(artist.Name, artist.Genre, artist.Songs);

            TempData[GlobalMessageKey] = "You added an Artist!";

            return RedirectToAction("Catalogue", "Artists", new { area = "" });
        }

        public IActionResult Delete()
        {
            return View(new DeleteArtistFormModel { });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(DeleteArtistFormModel artist)
        {
            if (!this.artists.Exists(artist.Name))
            {
                this.ModelState.AddModelError(nameof(artist.Name), "Artist with that name doesn't exist");
            }

            if (!ModelState.IsValid)
            {
                return View(artist);
            }
            var artistName = artist.Name;
            this.artists.Delete(artistName);

            TempData[GlobalMessageKey] = "You deleted an Artist";

            return RedirectToAction("Catalogue", "Artists", new { area = "" });
        }

    }
}

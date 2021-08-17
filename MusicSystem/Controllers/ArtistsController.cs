using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Artists;
using MusicSystem.Services;
using MusicSystem.Services.Partners;
using static Constants;

namespace MusicSystem.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistService artists;
        private readonly IPartnerService partners;

        public ArtistsController(IArtistService artists, IPartnerService partners)
        {
            this.artists = artists;
            this.partners = partners;
        }

        [Authorize]
        public IActionResult Catalogue([FromQuery] CatalogueArtistsQueryModel query)
        {
            var queryResult = this.artists.Catalogue(
                query.Artist,
                query.SearchTerm,
                query.CurrentPage,
                CatalogueArtistsQueryModel.ArtistsPerPage);

            query.TotalArtists = queryResult.TotalArtists;
            query.Artists = queryResult.Artists;

            return View(query);
        }

        [Authorize]
        public IActionResult ArtistSongs(string id)
        {
            var artist = artists.GetArtistSongs(id);

            return View(artist);
        }

        [Authorize]
        public IActionResult Add()
        {
            if (!this.partners.IsPartner(this.User.GetId()))
            {
                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            return View(new AddArtistFormModel {});
        }

        [HttpPost]
        public IActionResult Add(AddArtistFormModel artist)
        {
            var partnerId = this.partners.IdByUser(this.User.GetId());

            if (partnerId == null)
            {
                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (this.artists.Exists(artist.Name))
            {
                this.ModelState.AddModelError(nameof(artist.Name), "Artist with this name already exists");
            }

            if (!ModelState.IsValid)
            {
                return View(artist);
            }

            this.artists.Add(artist.Name,artist.Genre, artist.Songs);

            TempData[GlobalMessageKey] = "You added an Artist!";

            return RedirectToAction(nameof(Catalogue));
        }
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Artists;
using MusicSystem.Services;
using MusicSystem.Services.Partners;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Catalogue([FromQuery] CatalogueArtistsQueryModel query)
        {
            var queryResult = await this.artists.CatalogueAsync(
                query.Artist,
                query.SearchTerm,
                query.CurrentPage,
                CatalogueArtistsQueryModel.ArtistsPerPage);

            query.TotalArtists = queryResult.TotalArtists;
            query.Artists = queryResult.Artists;

            return View(query);
        }

        [Authorize]
        public async Task<IActionResult> ArtistSongsAsync(string id)
        {
            var artist = await artists.GetArtistSongsAsync(id);

            return View(artist);
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            if (!await this.partners.IsPartnerAsync(this.User.GetId()))
            {
                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            return View(new AddArtistFormModel {});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddArtistFormModel artist)
        {
            var partnerId = this.partners.IdByUserAsync(this.User.GetId());

            if (partnerId == null)
            {
                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (await this.artists.ExistsAsync(artist.Name))
            {
                await Task.Run(() => this.ModelState.AddModelError(nameof(artist.Name), "Artist with this name already exists"));
            }

            if (!ModelState.IsValid)
            {
                return View(artist);
            }

            await this.artists.AddAsync(artist.Name, artist.Genre, artist.Songs);

            TempData[GlobalMessageKey] = "You added an Artist!";

            return RedirectToAction(nameof(Catalogue));
        }
    }
}


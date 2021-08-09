using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models.Artists;
using MusicSystem.Services;

namespace MusicSystem.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistService artists;

        public ArtistsController(IArtistService artists)
        {
            this.artists = artists;
        }

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

        public IActionResult ArtistSongs(string id)
        {
            var artist = artists.GetArtistSongs(id);

            return View(artist);
        }
    }
}


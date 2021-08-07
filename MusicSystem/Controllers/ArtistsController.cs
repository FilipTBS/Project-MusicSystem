using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models.Artists;
using MusicSystem.Services;

namespace MusicSystem.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistService artists;
        private readonly IMapper mapper;

        public ArtistsController(IArtistService artists, IMapper mapper)
        {
            this.artists = artists;
            this.mapper = mapper;
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


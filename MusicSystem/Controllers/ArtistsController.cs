using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Models;
using MusicSystem.Models.Artists;
using MusicSystem.Services;
using System.Collections.Generic;
using System.Linq;

namespace MusicSystem.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly MusicSystemDbContext data;
        private readonly IArtistService artists;
        private readonly IMapper mapper;

        public ArtistsController(MusicSystemDbContext data,
            IArtistService artists, IMapper mapper)
        {
            this.data = data;
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


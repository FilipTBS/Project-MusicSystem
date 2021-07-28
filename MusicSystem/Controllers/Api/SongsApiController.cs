using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models.Api;
using MusicSystem.Services.Songs;

namespace MusicSystem.Controllers.Api
{
    [ApiController]
    [Route("api/songs")]
    public class SongsApiController : Controller
    {
        private readonly ISongService songs;

        public SongsApiController(ISongService songs)
            => this.songs = songs;

        [HttpGet]
        public SongQueryServiceModel All([FromQuery] AllSongsApiRequestModel query)
            => this.songs.All(
                query.Artist,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.SongsPerPage);
    }
}

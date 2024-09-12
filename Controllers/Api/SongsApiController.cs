using Microsoft.AspNetCore.Mvc;
using MusicSystem.Models.Api;
using MusicSystem.Services.Songs;
using System.Threading.Tasks;

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
        public async Task<SongQueryServiceModel> All([FromQuery] AllSongsApiRequestModel query)
        {
            return await this.songs.AllAsync(
                query.Artist,
                query.SearchTerm,
                query.CurrentPage,
                query.SongsPerPage);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Songs;
using MusicSystem.Services.Songs;
using System.Threading.Tasks;
using static Constants;

namespace MusicSystem.Areas.Admin.Controllers
{
    public class SongsController : AdminController
    {
        private readonly ISongService songs;
        private readonly IMapper mapper;

        public SongsController(ISongService songs,
            IMapper mapper)
        {
            this.songs = songs;
            this.mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage([FromQuery] AllSongsQueryModel query)
        {
            var queryResult = await this.songs.AllAsync(
                query.Artist,
                query.SearchTerm,
                query.CurrentPage,
                AllSongsQueryModel.SongsPerPage);

            var songArtists = await this.songs.AllArtistsAsync();

            query.Artists = songArtists;
            query.TotalSongs = queryResult.TotalSongs;
            query.Songs = queryResult.Songs;

            return View(query);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ChangeVisibility(string id)
        {
            this.songs.ChangeVisilityAsync(id);

            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            return View(new SongFormModel
            {
                Artists = await this.songs.AllArtistsAsync()
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(SongFormModel song)
        {
            if (!await this.songs.ArtistExistsAsync(song.ArtistId))
            {
                this.ModelState.AddModelError(nameof(song.ArtistId), "This artist does not exist!");
            }

            if (!ModelState.IsValid)
            {
                song.Artists = await this.songs.AllArtistsAsync();

                return View(song);
            }

            await this.songs.AddAsync(song.Title,
                song.ArtistId,
                song.Genre,
                song.Lyrics,
                song.SongUrl,
                null,
                this.User.IsAdmin());

            TempData[GlobalMessageKey] = "You added a song!";

            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var song = await this.songs.GetSongInfoAsync(id);

            var songForm = this.mapper.Map<SongFormModel>(song);
            songForm.Artists = await this.songs.AllArtistsAsync();
            return View(songForm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(string id, SongFormModel song)
        {
            if (!await this.songs.ArtistExistsAsync(song.ArtistId))
            {
                this.ModelState.AddModelError(nameof(song.ArtistId), "The artist doesn't exist!");
            }

            if (!ModelState.IsValid)
            {
                song.Artists = await this.songs.AllArtistsAsync();

                return View(song);
            }

            await this.songs.EditAsync(
                id,
                song.Title,
                song.ArtistId,
                song.Lyrics,
                song.SongUrl,
                song.Genre,
                this.User.IsAdmin());

            TempData[GlobalMessageKey] = $"You edited a song!";

            return RedirectToAction(nameof(Manage));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            await this.songs.DeleteAsync(id);
            TempData[GlobalMessageKey] = $"You deleted a song!";

            return RedirectToAction(nameof(Manage));
        }
    }
}

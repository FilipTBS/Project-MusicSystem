using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Infrastructure;
using MusicSystem.Models.Songs;
using MusicSystem.Services.Songs;
using static MusicSystem.Constants;

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

        public IActionResult Manage() 
            => View(this.songs.All(approvedOnly: false).Songs);

        public IActionResult ChangeVisibility(string id)
        {
            this.songs.ChangeVisility(id);

            return RedirectToAction(nameof(Manage));
        }

        public IActionResult Add()
        {
            return View(new SongFormModel
            {
                Artists = this.songs.AllArtists()
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(SongFormModel song)
        {
            if (!this.songs.ArtistExists(song.ArtistId))
            {
                this.ModelState.AddModelError(nameof(song.ArtistId), "This artist does not exist!");
            }

            if (!ModelState.IsValid)
            {
                song.Artists = this.songs.AllArtists();

                return View(song);
            }

            this.songs.Add(song.Title,
                song.ArtistId,
                song.Genre,
                song.Lyrics,
                song.SongUrl,
                null,
                this.User.IsAdmin());

            TempData[GlobalMessageKey] = "You added a song!";

            return RedirectToAction(nameof(Manage));
        }

        public IActionResult All([FromQuery] AllSongsQueryModel query)
        {
            var queryResult = this.songs.All(
                query.Artist,
                query.SearchTerm,
                query.CurrentPage,
                AllSongsQueryModel.SongsPerPage);

            var songArtists = this.songs.AllArtists();

            query.Artists = songArtists;
            query.TotalSongs = queryResult.TotalSongs;
            query.Songs = queryResult.Songs;

            return View(query);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            var song = this.songs.GetSongInfo(id);

            var songForm = this.mapper.Map<SongFormModel>(song);
            songForm.Artists = this.songs.AllArtists();
            return View(songForm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(string id, SongFormModel song)
        {
            if (!this.songs.ArtistExists(song.ArtistId))
            {
                this.ModelState.AddModelError(nameof(song.ArtistId), "The artist doesn't exist!");
            }

            if (!ModelState.IsValid)
            {
                song.Artists = this.songs.AllArtists();

                return View(song);
            }

            this.songs.Edit(
                id,
                song.Title,
                song.ArtistId,
                song.Lyrics,
                song.SongUrl,
                song.Genre,
                this.User.IsAdmin());

            TempData[GlobalMessageKey] = $"Song edited a song!";

            return RedirectToAction("Songs", "All");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            this.songs.Delete(id);
            TempData[GlobalMessageKey] = $"You deleted a song!";

            return RedirectToAction(nameof(Manage));
        }
    }
}

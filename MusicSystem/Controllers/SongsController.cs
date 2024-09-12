using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Songs;
using MusicSystem.Services.Curators;
using MusicSystem.Services.Songs;
using System;
using System.Threading.Tasks;
using static Constants;

namespace MusicSystem.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongService songs;
        private readonly ICuratorService curators;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public SongsController(ISongService songs,
            ICuratorService curators, IMapper mapper,
            IMemoryCache cache)
        {
            this.songs = songs;
            this.curators = curators;
            this.mapper = mapper;
            this.cache = cache;
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            if (!await this.curators.IsCuratorAsync(this.User.GetId()))
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

            return View(new SongFormModel
            {
                Artists = await this.songs.AllArtistsAsync()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(SongFormModel song)
        {
            var curatorId = await this.curators.IdByUserAsync(this.User.GetId());

            if (curatorId == null)
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

            if (!await this.songs.ArtistExistsAsync(song.ArtistId))
            {
                await Task.Run(() => this.ModelState.AddModelError(nameof(song.ArtistId), "This artist does not exist!"));
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
                curatorId,
                this.User.IsAdmin());

            TempData[GlobalMessageKey] = "You added a song and is awaiting for approval!";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> All([FromQuery] AllSongsQueryModel query)
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

        [Authorize]
        public async Task<IActionResult> Mine()
        {
            var mySongs = await this.songs.ByUserAsync(this.User.GetId());

            return View(mySongs);
        }

        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            var userId = this.User.GetId();

            if (!await this.curators.IsCuratorAsync(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Dealers");
            }

            var song = await this.songs.GetSongInfoAsync(id);

            if (song.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var songForm = this.mapper.Map<SongFormModel>(song);
            songForm.Artists = await this.songs.AllArtistsAsync();
            return View(songForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditAsync(string id, SongFormModel song)
        {
            var curatorId = await this.curators.IdByUserAsync(this.User.GetId());

            if (curatorId == null && !User.IsAdmin())
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

            if (!await this.songs.ArtistExistsAsync(song.ArtistId))
            {
                this.ModelState.AddModelError(nameof(song.ArtistId), "The artist doesn't exist!");
            }

            if (!ModelState.IsValid)
            {
                song.Artists = await this.songs.AllArtistsAsync();

                return View(song);
            }

            if (!await this.songs.IsByCuratorAsync(id, curatorId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            await this.songs.EditAsync(
                id,
                song.Title,
                song.ArtistId,
                song.Lyrics,
                song.SongUrl,
                song.Genre,
                this.User.IsAdmin());

            TempData[GlobalMessageKey] = $"Song edited{(this.User.IsAdmin() ? string.Empty : " and is awaiting for approval")}!";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var curatorId = await this.curators.IdByUserAsync(this.User.GetId());

            if (curatorId == null && !User.IsAdmin())
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

            if (!await this.songs.IsByCuratorAsync(id, curatorId))
            {
                return Unauthorized();
            }

            await this.songs.DeleteAsync(id);
            TempData[GlobalMessageKey] = $"Your song was deleted!";

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public async Task<IActionResult> Lyrics(string id)
        {
            var song = await this.songs.GetLyricsAsync(id);

            var songLyrics = this.cache
            .Get<SongLyricsViewModel>("SongLyricsCache");
            if (songLyrics == null)
            {
                var options = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                this.cache.Set("SongLyricsCache", songLyrics, options);
            }

            songLyrics = new SongLyricsViewModel
            {
                Id = id,
                Lyrics = song.Lyrics
            };

            return View(songLyrics);
        }
    }
}
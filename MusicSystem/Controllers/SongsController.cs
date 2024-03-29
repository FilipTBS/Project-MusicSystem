﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MusicSystem.Infrastructure.Extensions;
using MusicSystem.Models.Songs;
using MusicSystem.Services.Curators;
using MusicSystem.Services.Songs;
using System;
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
        public IActionResult Add()
        {
            if (!this.curators.IsCurator(this.User.GetId()))
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

            return View(new SongFormModel
            {
                Artists = this.songs.AllArtists()
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(SongFormModel song)
        {
            var curatorId = this.curators.IdByUser(this.User.GetId());

            if (curatorId == null)
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

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
                curatorId,
                this.User.IsAdmin());

            TempData[GlobalMessageKey] = "You added a song and is awaiting for approval!";

            return RedirectToAction(nameof(All));
        }

        [Authorize]
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

        [Authorize]
        public IActionResult Mine()
        {
            var mySongs = this.songs.ByUser(this.User.GetId());

            return View(mySongs);
        }

        [Authorize]
        public IActionResult Edit(string id)
        {
            var userId = this.User.GetId();

            if (!this.curators.IsCurator(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Dealers");
            }

            var song = this.songs.GetSongInfo(id);

            if (song.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var songForm = this.mapper.Map<SongFormModel>(song);
            songForm.Artists = this.songs.AllArtists();
            return View(songForm);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(string id, SongFormModel song)
        {
            var curatorId = this.curators.IdByUser(this.User.GetId());

            if (curatorId == null && !User.IsAdmin())
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

            if (!this.songs.ArtistExists(song.ArtistId))
            {
                this.ModelState.AddModelError(nameof(song.ArtistId), "The artist doesn't exist!");
            }

            if (!ModelState.IsValid)
            {
                song.Artists = this.songs.AllArtists();

                return View(song);
            }

            if (!this.songs.IsByCurator(id, curatorId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.songs.Edit(
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
        public IActionResult Delete(string id)
        {
            var curatorId = this.curators.IdByUser(this.User.GetId());

            if (curatorId == null && !User.IsAdmin())
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

            if (!this.songs.IsByCurator(id, curatorId))
            {
                return Unauthorized();
            }

            this.songs.Delete(id);
            TempData[GlobalMessageKey] = $"Your song was deleted!";

            return RedirectToAction(nameof(Mine));
        }

        [Authorize]
        public IActionResult Lyrics(string id)
        {
            var song = this.songs.GetLyrics(id);

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
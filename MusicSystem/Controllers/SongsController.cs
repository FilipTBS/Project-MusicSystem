using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Models.Songs;
using System.Collections.Generic;
using System.Linq;

namespace MusicSystem.Controllers
{
    public class SongsController : Controller
    {
        private readonly MusicSystemDbContext data;

        public SongsController(MusicSystemDbContext data) => this.data = data;

        public IActionResult Add() => View(new AddSongFormModel
        {
            Artists = this.GetSongArtists()
        });

        [HttpPost]
        public IActionResult Add(AddSongFormModel song)
        {
            if (!this.data.Artists.Any(x => x.Id == song.ArtistId))
            {
                this.ModelState.AddModelError(nameof(song.ArtistId), "Artist does not exist!");
            }

            if (!ModelState.IsValid)
            {
                song.Artists = this.GetSongArtists();

                return View(song);
            }

            var songObject = new Song
            {
                Title = song.Title,
                ArtistId = song.ArtistId,
                Genre = song.Genre,
                Lyrics = song.Lyrics,
                Language = song.Language,
                SongUrl = song.SongUrl
            };

            this.data.Songs.Add(songObject);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<SongArtistViewModel> GetSongArtists()
        => this.data.Artists.Select(x => new SongArtistViewModel
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        public IActionResult All()
        {
            var songs = this.data.Songs
                .OrderBy(x => x.Title)
                .Select(x => new SongListingViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Artist = x.Artist.Name,
                Genre = x.Genre,
                SongUrl = x.SongUrl,
                Lyrics = x.Lyrics,
                Likes = x.Likes
            }).ToList();

            return View(songs);
        }

        public IActionResult Lyrics(string id)
        {
            var song = this.data.Songs.Where(x => x.Id == id).FirstOrDefault();

            var songLyrics = new SongLyricsViewModel
            {
                Id = id,
                Lyrics = song.Lyrics
            };

            return View(songLyrics);
        }
    }
}

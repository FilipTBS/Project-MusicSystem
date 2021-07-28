using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Infrastructure;
using MusicSystem.Models.Songs;
using MusicSystem.Services.Curators;
using MusicSystem.Services.Songs;
using System.Collections.Generic;
using System.Linq;

namespace MusicSystem.Controllers
{
    public class SongsController : Controller
    {
        private readonly ISongService songs;
        private readonly ICuratorService curators;
        private readonly MusicSystemDbContext data;

        public SongsController(ISongService songs, 
            MusicSystemDbContext data,
            ICuratorService curators)
        {
            this.data = data;
            this.songs = songs;
            this.curators = curators;
        }
                   

        [Authorize]
        public IActionResult Add()
        {
            if (!this.curators.IsCurator(this.User.GetId()))
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

            return View(new AddSongFormModel
            {
                Artists = this.GetSongArtists()
            });
            
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddSongFormModel song)
        {
            var curatorId = this.data.Curators
                .Where(x => x.UserId == this.User.GetId())
                .Select(x => x.Id).FirstOrDefault();



            if (curatorId == 0)
            {
                return RedirectToAction(nameof(CuratorsController.Become), "Curators");
            }

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
                SongUrl = song.SongUrl,
                CuratorId = curatorId,
                CuratorVerified = true
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

        public IActionResult All([FromQuery]AllSongsQueryModel query)
        {
            var queryResult = this.songs.All(
                query.Artist,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllSongsQueryModel.SongsPerPage);
            //string artist,
            //string searchTerm, SongSorting sorting,
            //int currentPage, int songsPerPage

            var songArtists = this.songs.AllArtists();

            query.Artists = songArtists;
            query.TotalSongs = queryResult.TotalSongs;
            query.Songs = queryResult.Songs;

            return View(query);
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

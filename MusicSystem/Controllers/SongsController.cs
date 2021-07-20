﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Infrastructure;
using MusicSystem.Models.Songs;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MusicSystem.Controllers
{
    public class SongsController : Controller
    {
        private readonly MusicSystemDbContext data;

        public SongsController(MusicSystemDbContext data)
        => this.data = data;
            

        [Authorize]
        public IActionResult Add()
        {
            if (!this.UserIsCurator())
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

        public IActionResult All([FromQuery]SongsQueryModel query)
        {
            var songsQuery = this.data.Songs.AsQueryable();


            //To check the filtering and search (also in Views -> Songs -> All)
             /*
            if (!string.IsNullOrWhiteSpace(query.Artist))
            {
                songsQuery = songsQuery.Where(x => x.Artist.Name == query.Artist);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                songsQuery = songsQuery
                    .Where(x => 
                    (x.Artist.Name + " " + x.Title).ToLower()
                    .Contains(query.SearchTerm.ToLower()));
            }*/

            var totalSongs = songsQuery.Count();

            var songs = this.data.Songs
                .Skip((query.CurrentPage - 1) * SongsQueryModel.SongsPerPage)
                .Take(SongsQueryModel.SongsPerPage)
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

            var songArtist = this.data.Songs
                .Select(x => x.Artist.Name)
                .Distinct().OrderBy(x => x).ToList();

            query.TotalSongs = totalSongs;
            query.Artists = songArtist;
            query.Songs = songs;

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

        private bool UserIsCurator()
        => this.data.Curators.Any(x => x.UserId == this.User.GetId());
    }
}

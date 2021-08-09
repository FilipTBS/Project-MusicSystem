﻿using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Models.Songs;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace MusicSystem.Services.Songs
{
    public class SongService : ISongService
    {
        private readonly MusicSystemDbContext data;
        private readonly IConfigurationProvider mapper;

        public SongService(MusicSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public string Add(string title, string artistId,
        string genre, string lyrics,
        string songUrl, int curatorId)
        {
            var songData = new Song
            {
                Title = title,
                ArtistId = artistId,
                Genre = genre,
                Lyrics = lyrics,
                SongUrl = songUrl,
                CuratorId = curatorId,
                IsApproved = false
            };

            this.data.Songs.Add(songData);
            this.data.SaveChanges();

            return songData.Id;
        }

        public SongQueryServiceModel All(string artist = null,
            string searchTerm = null,
            SongSorting sorting = SongSorting.DateCreated,
            int currentPage = 1, 
            int songsPerPage = 10, 
            bool approvedOnly = true)
        {
            var songsQuery = this.data.Songs
                .Where(x => !x.IsApproved || x.IsApproved);

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

            var songs = GetSongs(songsQuery
                .Skip((currentPage - 1) * songsPerPage)
                .Take(songsPerPage));

            return new SongQueryServiceModel
            {
                TotalSongs = totalSongs,
                CurrentPage = currentPage,
                SongsPerPage = songsPerPage,
                Songs = songs
            };
        }

    public SongLyricsServiceModel GetLyrics(string songId)
    =>      this.data.Songs
                .Where(c => c.Id == songId)
                .Select(c => new SongLyricsServiceModel
                {
                    Id = c.Id,
                    Lyrics = c.Lyrics
                })
                .FirstOrDefault();
    public bool Edit(string songId, string title, 
            string artistId, string lyrics, string songUrl, 
            string genre, bool isApproved)
        {
            var songData = this.data.Songs.Find(songId);

            if (songData == null)
            {
                return false;
            }

            songData.Title = title;
            songData.ArtistId = artistId;
            songData.SongUrl = songUrl;
            songData.Lyrics = lyrics;
            songData.Genre = genre;
            songData.IsApproved = isApproved;

            this.data.SaveChanges();
            return true;
        }

        public SongInfoServiceModel GetSongInfo(string songId)
        => this.data.Songs
                .Where(c => c.Id == songId)
                .ProjectTo<SongInfoServiceModel>(this.mapper)
                .FirstOrDefault();

        public IEnumerable<LatestSongServiceModel> Latest()
        => this.data.Songs.Where(x => x.IsApproved)
        .OrderByDescending(c => c.Id)
        .ProjectTo<LatestSongServiceModel>(this.mapper)
        .Take(3)
        .ToList();

        private IEnumerable<SongServiceModel> GetSongs(IQueryable<Song> songQuery)
        => songQuery.ProjectTo<SongServiceModel>(this.mapper)
           .ToList();

        public void ChangeVisility(string songId)
        {
            var song = this.data.Songs.Find(songId);

            song.IsApproved = !song.IsApproved;

            this.data.SaveChanges();
        }

        public IEnumerable<string> AllTitles()
         => this.data.Songs
                .Select(x => x.Title)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

        public IEnumerable<SongArtistServiceModel> AllArtists()
        => this.data.Artists
           .ProjectTo<SongArtistServiceModel>(this.mapper).ToList();
        
        public bool ArtistExists(string artistId)
        => this.data.Artists
                .Any(x => x.Id == artistId);

        public bool IsByCurator(string songId, int curatorId)
        => this.data.Songs
                .Any(c => c.Id == songId && c.CuratorId == curatorId);

        public IEnumerable<SongServiceModel> ByUser(string userId)
        => GetSongs(this.data.Songs
                .Where(c => c.Curator.UserId == userId));

    }

}



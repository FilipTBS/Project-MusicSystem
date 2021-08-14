using MusicSystem.Data;
using MusicSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

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
        string songUrl, string curatorId, bool isApproved)
        {
            var songData = new Song
            {
                Title = title,
                ArtistId = artistId,
                Genre = genre,
                Lyrics = lyrics,
                SongUrl = songUrl,
                CuratorId = curatorId,
                IsApproved = isApproved
            };

            this.data.Songs.Add(songData);
            this.data.SaveChanges();

            return songData.Id;
        }

        public SongQueryServiceModel All(string artist = null,
            string searchTerm = null,
            int currentPage = 1, 
            int songsPerPage = 10, 
            bool approvedOnly = true)
        {
            var songsQuery = this.data.Songs.Include(x => x.Artist)
                .Where(x => !x.IsApproved || x.IsApproved);

            if (!string.IsNullOrWhiteSpace(artist))
            {
                songsQuery = songsQuery.Where(c => c.Artist.Name == artist);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                songsQuery = songsQuery.Where(c =>
                    (c.Artist.Name + " " + c.Title).ToLower()
                    .Contains(searchTerm.ToLower()) ||
                    c.Genre.ToLower().Contains(searchTerm.ToLower()));
            }

            songsQuery = songsQuery.OrderByDescending(c => c.Id);

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

        public void Delete(string songId)
        {
            var song = this.data.Songs.Find(songId);

            if (song != null)
            {
                this.data.Songs.Remove(song);

                this.data.SaveChanges();
            }       
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
        .Take(3).ToList();

        private IEnumerable<SongServiceModel> GetSongs(IQueryable<Song> songQuery)
        => songQuery.ProjectTo<SongServiceModel>(this.mapper)
           .ToList();

        public void ChangeVisility(string songId)
        {
            var song = this.data.Songs.Find(songId);

            song.IsApproved = !song.IsApproved;

            this.data.SaveChanges();
        }

        public IEnumerable<SongArtistServiceModel> AllArtists()
        => this.data.Artists
           .ProjectTo<SongArtistServiceModel>(this.mapper).ToList();
        
        public bool ArtistExists(string artistId)
        => this.data.Artists
                .Any(x => x.Id == artistId);

        public bool IsByCurator(string songId, string curatorId)
            => this.data.Songs.Any(c => c.Id == songId && c.CuratorId == curatorId);

        public IEnumerable<SongServiceModel> ByUser(string userId)
        => GetSongs(this.data.Songs
                .Where(c => c.Curator.UserId == userId));

    }

}



using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Models.Songs;
using System.Collections.Generic;
using System.Linq;

namespace MusicSystem.Services.Songs
{
    public class SongService : ISongService
    {
        private readonly MusicSystemDbContext data;

        public SongService(MusicSystemDbContext data)
        {
            this.data = data;
        }

        public string Create(string title, string artistId,
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
                CuratorId = curatorId
            };

            this.data.Songs.Add(songData);
            this.data.SaveChanges();

            return songData.Id;
        }

        public SongQueryServiceModel All(string artist,
            string searchTerm, SongSorting sorting,
            int currentPage, int songsPerPage)
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

        public SongLyricsServiceModel Lyrics(string songId)
        => this.data.Songs
                .Where(c => c.Id == songId)
                .Select(c => new SongLyricsServiceModel
                {
                    Id = c.Id,
                    Lyrics = c.Lyrics
                })
                .FirstOrDefault();

        public bool Edit(string songId, string title, 
            string artistId, string lyrics, string songUrl, string genre)
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

            this.data.SaveChanges();

            return true;
        }

        public SongInfoServiceModel GetSongInfo(string songId)
        => this.data.Songs
                .Where(c => c.Id == songId)
                .Select(c => new SongInfoServiceModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    ArtistId = c.ArtistId,
                    Lyrics = c.Lyrics,
                    Genre = c.Genre,
                    SongUrl = c.SongUrl,
                    ArtistName = c.Artist.Name,
                    CuratorId = c.CuratorId,
                    CuratorName = c.Curator.Name,
                    UserId = c.Curator.UserId
                })
                .FirstOrDefault();


        private static IEnumerable<SongServiceModel> GetSongs(IQueryable<Song> songQuery)
        => songQuery
        .Select(s => new SongServiceModel
        {
              Id = s.Id,
              Title = s.Title,
              ArtistName = s.Artist.Name,
              Genre = s.Genre,
              Lyrics = s.Lyrics,
              SongUrl = s.SongUrl,
              Likes = s.Likes

        }).ToList();

        public IEnumerable<string> AllTitles()
         => this.data.Songs
                .Select(x => x.Title)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

        public IEnumerable<SongArtistModel> AllArtists()
        => this.data.Artists
                .Select(c => new SongArtistModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();

        public bool ArtistExists(string artistId)
        => this.data.Songs
                .Any(c => c.Artist.Id == artistId);

        public bool IsByCurator(string songId, int curatorId)
        => this.data.Songs
                .Any(c => c.Id == songId && c.CuratorId == curatorId);

        public IEnumerable<SongServiceModel> ByUser(string userId)
        => GetSongs(this.data.Songs
                .Where(c => c.Curator.UserId == userId));


    }

}



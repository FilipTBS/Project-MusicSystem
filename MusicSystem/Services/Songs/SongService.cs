using MusicSystem.Data;
using MusicSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        public async Task<string> AddAsync(string title, string artistId,
        string genre, string lyrics,
        string songUrl, string curatorId, bool isApproved)
        {
            var songData = new Data.Models.Song
            {
                Title = title,
                ArtistId = artistId,
                Genre = genre,
                Lyrics = lyrics,
                SongUrl = songUrl,
                CuratorId = curatorId,
                IsApproved = isApproved
            };

            await this.data.Songs.AddAsync(songData);
            await this.data.SaveChangesAsync();

            return songData.Id;
        }

        public async Task<SongQueryServiceModel> AllAsync(string artist = null,
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

            //songsQuery = songsQuery.OrderByDescending(c => c.Id);

            var totalSongs = await songsQuery.CountAsync();

            var songs = await GetSongsAsync(songsQuery
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

    public async Task<SongLyricsServiceModel> GetLyricsAsync(string songId)
    =>  await this.data.Songs
                .Where(c => c.Id == songId)
                .Select(c => new SongLyricsServiceModel
                {
                    Id = c.Id,
                    Lyrics = c.Lyrics
                })
                .FirstOrDefaultAsync();
    public async Task<bool> EditAsync(string songId, string title, 
            string artistId, string lyrics, string songUrl, 
            string genre, bool isApproved)
        {
            var songData = await this.data.Songs.FindAsync(songId);

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

            await this.data.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(string songId)
        {
            var song = await this.data.Songs.FindAsync(songId);

            if (song != null)
            {
                this.data.Songs.Remove(song);

                await this.data.SaveChangesAsync();
            }
        }

        public async Task<SongInfoServiceModel> GetSongInfoAsync(string songId)
        {
            return await this.data.Songs
                .Where(c => c.Id == songId)
                .ProjectTo<SongInfoServiceModel>(this.mapper)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ExampleSongServiceModel>> ExampleAsync()
        {
           return await this.data.Songs.Where(x => x.IsApproved)
            .OrderBy(c => c.Title)
            .ProjectTo<ExampleSongServiceModel>(this.mapper)
            .Take(3).ToListAsync();
        }

        private async Task<ICollection<SongServiceModel>> GetSongsAsync(IQueryable<Data.Models.Song> songQuery)
        {
            return await songQuery.ProjectTo<SongServiceModel>(this.mapper)
           .ToListAsync();
        }

        public async Task ChangeVisilityAsync(string songId)
        {
            var song = await this.data.Songs.FindAsync(songId);

            song.IsApproved = !song.IsApproved;

            await this.data.SaveChangesAsync();
        }

        public async Task<ICollection<SongArtistServiceModel>> AllArtistsAsync()
        {
            return await this.data.Artists
           .ProjectTo<SongArtistServiceModel>(this.mapper).ToListAsync();
        }
        
        public async Task<bool> ArtistExistsAsync(string artistId)
        {
            return await this.data.Artists
                .AnyAsync(x => x.Id == artistId);
        }

        public async Task<bool> IsByCuratorAsync(string songId, string curatorId)
        {
            return await this.data.Songs.AnyAsync(c => c.Id == songId && c.CuratorId == curatorId);
        }

        public async Task<ICollection<SongServiceModel>> ByUserAsync(string userId)
        {
            return await GetSongsAsync(this.data.Songs
                .Where(c => c.Curator.UserId == userId));
        }

    }

}



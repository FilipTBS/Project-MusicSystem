using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Models.Artists;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicSystem.Services.Artists
{
    public class ArtistService : IArtistService
    {
        private readonly MusicSystemDbContext data;

        public ArtistService(MusicSystemDbContext data)
        {
            this.data = data;
        }

        public async Task<ArtistQueryServiceModel> CatalogueAsync(string artist,
            string searchTerm, int currentPage, int artistsPerPage)
        {
            var artistsQuery = this.data.Artists.AsQueryable();
            var totalArtists = await artistsQuery.CountAsync();
            var artists = await GetArtistsAsync(artistsQuery
                             .Skip((currentPage - 1) * artistsPerPage)
                             .Take(artistsPerPage));

            return new ArtistQueryServiceModel
            {
                TotalArtists = totalArtists,
                CurrentPage = currentPage,
                ArtistsPerPage = artistsPerPage,
                Artists = artists
            };
        }


        private static async Task<ICollection<ArtistServiceModel>> GetArtistsAsync(IQueryable<Artist> artistQuery)
        {
            return await artistQuery.Select(x => new ArtistServiceModel
            {
                Id = x.Id,
                Name = x.Name,
                Genre = x.Genre
            }).ToListAsync();
        }

        public async Task<ArtistSongsViewModel> GetArtistSongsAsync(string id)
        {
            var artist = await this.data.Artists.Where(x => x.Id == id).FirstAsync();
            var songTitles = await this.data.Songs.Where(x => x.ArtistId == id)
                .Select(x => x.Title).ToListAsync();
            var artistSongs = new ArtistSongsViewModel
            {
                SongTitles = songTitles
            };
            return artistSongs;      
        }

        public async Task<bool> ExistsAsync(string name)
        => await this.data.Artists
        .AnyAsync(x => x.Name == name);

        public async Task<Artist> FindArtistAsync(string name)
         => await this.data.Artists
         .FirstOrDefaultAsync(x => x.Name == name);

        public async Task<string> AddAsync(string name, string genre, ICollection<Song> songs)
        {
            var artistData = new Artist
            {
                Name = name,
                Genre = genre,
                Songs = songs       
            };

            await this.data.Artists.AddAsync(artistData);
            await this.data.SaveChangesAsync();

            return artistData.Id;
        }

        public async Task DeleteAsync(string name)
        {
            var artistName = await this.data.Artists.Where(x => x.Name == name).FirstOrDefaultAsync();

            if (artistName != null)
            {
                this.data.Artists.Remove(artistName);

                await this.data.SaveChangesAsync();
            }
        }
    }
}

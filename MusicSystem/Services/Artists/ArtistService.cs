using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Models.Artists;
using System.Collections.Generic;
using System.Linq;

namespace MusicSystem.Services.Artists
{
    public class ArtistService : IArtistService
    {
        private readonly MusicSystemDbContext data;

        public ArtistService(MusicSystemDbContext data)
        {
            this.data = data;
        }

        public ArtistQueryServiceModel Catalogue(string artist,
            string searchTerm, int currentPage, int artistsPerPage)
        {
            var artistsQuery = this.data.Artists.AsQueryable();
            var totalArtists = artistsQuery.Count();

            var artists = GetArtists(artistsQuery
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

        private static ICollection<ArtistServiceModel> GetArtists(IQueryable<Artist> artistQuery)
        => artistQuery.Select(x => new ArtistServiceModel
        {
            Id = x.Id,
            Name = x.Name,
            Genre = x.Genre

        }).ToList();

        public ArtistSongsViewModel GetArtistSongs(string id)
        {
            var artist = this.data.Artists.Where(x => x.Id == id).First();
            var songTitles = this.data.Songs.Where(x => x.ArtistId == id)
                .Select(x => x.Title).ToList();
            var artistSongs = new ArtistSongsViewModel
            {
                SongTitles = songTitles
            };
            return artistSongs;      
        }

        public bool Exists(string name)
        => this.data.Artists
        .Any(x => x.Name == name);

        public Artist FindArtist(string name)
         => this.data.Artists
         .FirstOrDefault(x => x.Name == name);

        public string Add(string name, string genre, ICollection<Song> songs)
        {
            var artistData = new Artist
            {
                Name = name,
                Genre = genre,
                Songs = songs       
            };

            this.data.Artists.Add(artistData);
            this.data.SaveChanges();

            return artistData.Id;
        }

        public void Delete(string name)
        {
            var artistName = this.data.Artists.Where(x => x.Name == name).FirstOrDefault();

            if (artistName != null)
            {
                this.data.Artists.Remove(artistName);

                this.data.SaveChanges();
            }
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MusicSystem.Data;
using MusicSystem.Data.Models;
using MusicSystem.Models;
using MusicSystem.Models.Artists;
using System.Collections.Generic;
using System.Linq;

namespace MusicSystem.Services.Artists
{
    public class ArtistService : IArtistService
    {
        private readonly MusicSystemDbContext data;
        private readonly IConfigurationProvider mapper;

        public ArtistService(MusicSystemDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public ArtistQueryServiceModel Catalogue(string artist,
            string searchTerm,int currentPage, int artistsPerPage)
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

        private static IEnumerable<ArtistServiceModel> GetArtists(IQueryable<Artist> artistQuery)
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
        /* var artist = this.data.Artists.Where(x => x.Id == id).First();

            var songTitles = artist.Songs.Select(x => x.Title);

            var artistSongs = new ArtistSongsViewModel
            {
                Id = id,
                SongTitles = songTitles
            };*/
    }
}

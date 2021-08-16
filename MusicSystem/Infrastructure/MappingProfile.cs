using AutoMapper;
using MusicSystem.Data.Models;
using MusicSystem.Services.Songs;

namespace MusicSystem.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Artist, SongArtistServiceModel>();

            this.CreateMap<Song, LatestSongServiceModel>();
            this.CreateMap<SongInfoServiceModel, SongServiceModel>();

            this.CreateMap<Song, SongServiceModel>()
                .ForMember(c => c.ArtistName, cfg => cfg.MapFrom(c => c.Artist.Name));

            this.CreateMap<Song, SongInfoServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Curator.UserId))
                .ForMember(c => c.ArtistName, cfg => cfg.MapFrom(c => c.Artist.Name));
        }
    }
}

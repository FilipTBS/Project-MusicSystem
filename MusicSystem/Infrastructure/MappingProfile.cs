using AutoMapper;
using MusicSystem.Data.Models;
using MusicSystem.Models.Songs;
using MusicSystem.Services.Songs;

namespace CarRentingSystem.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Song, LatestSongServiceModel>();
            this.CreateMap<SongInfoServiceModel, SongFormModel>();

            this.CreateMap<Song, SongInfoServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Curator.UserId));
        }
    }
}

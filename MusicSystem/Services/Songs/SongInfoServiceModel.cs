using MusicSystem.Models.Songs;

namespace MusicSystem.Services.Songs
{
    public class SongInfoServiceModel : SongServiceModel
    {
        public string ArtistId { get; init; }

        public int CuratorId { get; init; }

        public string CuratorName { get; init; }

        public string UserId { get; init; }
    }
}
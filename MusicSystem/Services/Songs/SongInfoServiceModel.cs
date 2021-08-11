namespace MusicSystem.Services.Songs
{
    public class SongInfoServiceModel : SongServiceModel
    {
        public string ArtistId { get; init; }

        public string CuratorId { get; init; }

        public string CuratorName { get; init; }

        public string UserId { get; init; }
    }
}
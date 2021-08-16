namespace MusicSystem.Models.Songs
{
    public class SongListingViewModel
    {
        public string Id { get; init; }

        public string Title { get; init; }

        public string Artist { get; init; }

        public string Genre { get; init; }

        public string SongUrl { get; init; }

        public string Lyrics { get; init; }

        public int Likes { get; set; }
    }
}

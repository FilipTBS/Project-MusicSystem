using MusicSystem.Models.Songs;

namespace MusicSystem.Models.Api
{
    public class AllSongsApiRequestModel
    {
        public string Artist { get; init; }

        public string SearchTerm { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int SongsPerPage { get; init; } = 10;
    }
}

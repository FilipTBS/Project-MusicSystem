namespace MusicSystem.Data
{
    public class Constants
    {
        public class Song
        {
            public const int SongTitleMinValue = 3;
            public const int SongTitleMaxValue = 20;

            public const int SongGenreMinValue = 3;
            public const int SongGenreMaxValue = 10;

            public const int SongLanguageMinValue = 2;
            public const int SongLanguageMaxValue = 10;

            public const int SongLyricsMinValue = 20;
        }

        public class Artist
        {
            public const int ArtistNameMinValue = 3;
            public const int ArtistNameMaxValue = 20;

            public const int ArtistGenreMinValue = 2;
            public const int ArtistGenreMaxValue = 10;
        }
        public class Curator
        {
            public const int CuratorNameMinValue = 3;
            public const int CuratorNameMaxValue = 30;
            public const int CuratorPhoneNumberMinValue = 5;
            public const int CuratorPhoneNumberMaxValue = 30;
        }
    }
}

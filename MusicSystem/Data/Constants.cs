﻿namespace MusicSystem.Data
{
    public class Constants
    {
        public class User
        {
            public const int FullNameMinLength = 5;
            public const int FullNameMaxLength = 50;
            public const int PasswordMinLength = 6;
            public const int PasswordMaxLength = 100;
        }

        public class Song
        {
            public const int SongTitleMinValue = 3;
            public const int SongTitleMaxValue = 30;

            public const int SongGenreMinValue = 3;
            public const int SongGenreMaxValue = 15;

            public const int SongLanguageMinValue = 2;
            public const int SongLanguageMaxValue = 15;

            public const int SongLyricsMinValue = 20;
            public const int SongLyricsMaxValue = int.MaxValue;
        }

        public class Artist
        {
            public const int ArtistNameMinValue = 3;
            public const int ArtistNameMaxValue = 30;

            public const int ArtistGenreMinValue = 2;
            public const int ArtistGenreMaxValue = 15;
        }
        public class Curator
        {
            public const int CuratorNicknameMinValue = 3;
            public const int CuratorNicknameMaxValue = 30;
        }

        public class Company
        {
            public const int CompanyNameMinValue = 4;
            public const int CompanyNameMaxValue = 50;
        }

        public class Partner : Company
        {
            
        }
        public class Feedback
        {
            public const int FeedbackSuggestionMinValue = 4;
            public const int FeedbackSuggestionMaxValue = 75;
        }
    }
}

﻿using System.Collections.Generic;

namespace MusicSystem.Services
{
    public class ArtistQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int ArtistsPerPage { get; init; }

        public int TotalArtists { get; init; }

        public ICollection<ArtistServiceModel> Artists { get; init; }
    }
}
using MusicSystem.Data.Models;
using System.Collections.Generic;

namespace MusicSystem.Services
{
    public class ArtistServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Genre { get; set; }
        public ICollection<Song> Songs { get; set; }
            = new List<Song>();
    }
}
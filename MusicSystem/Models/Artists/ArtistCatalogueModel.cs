using MusicSystem.Data.Models;
using System.Collections.Generic;

namespace MusicSystem.Models
{
    public class ArtistCatalogueModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Genre { get; set; }

        public ICollection<Song> Songs { get; set; } = new List<Song>();

    }
}

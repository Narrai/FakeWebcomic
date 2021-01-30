using System.Collections.Generic;

namespace FakeWebcomic.Storage.Models
{
    public class ComicBook : AEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int EditionNumber { get; set; }


        public ICollection<ComicPage> ComicPages { get; set; }

    }
}

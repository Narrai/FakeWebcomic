using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class ComicBookModel : AEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int EditionNumber { get; set; }

        public ICollection<ComicPageModel> ComicPages { get; set; }
    }
}

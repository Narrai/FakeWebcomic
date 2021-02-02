using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class ComicBookModel : AEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int EditionNumber { get; set; }
        public string Description {get;set;}

        public ICollection<ComicPageModel> ComicPages { get; set; }

        public ComicBookModel() {}

        public ComicBookModel(ComicBookViewModel model)
        {
            Title = model.Title;
            Author = model.Author;
            Genre = model.Genre;
            EditionNumber = model.EditionNumber;
            Description = model.Description;
            ComicPages = model.ComicPages;
        }
    }
}

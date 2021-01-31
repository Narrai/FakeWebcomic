using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class ComicAboutViewModel
    {
        public string Title { get; set; }
        public long WebcomicId {get;set;}
        public string Author { get; set; }
        public string Genre { get; set; }
        public int EditionNumber { get; set; }
        public string Description {get;set;}

        public int NumberOfPages {get;set;}

        ComicAboutViewModel(ComicBookModel comic)
        {
            Title = comic.Title;
            WebcomicId = comic.EntityId;
            Author = comic.Author;
            Genre = comic.Genre;
            EditionNumber = comic.EditionNumber;
            Description = comic.Description;
            NumberOfPages = comic.ComicPages.Length;
        }
    }
}

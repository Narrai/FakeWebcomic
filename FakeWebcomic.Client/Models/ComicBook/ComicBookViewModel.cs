using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FakeWebcomic.Client.Models
{
    public class ComicBookViewModel
    {
        [Required]
        public string Title { get; set; }
        public long WebcomicId {get;set;}
        [Required]
        public string Author { get; set; }
        public string Genre { get; set; }
        public int EditionNumber { get; set; }
        public string Description {get;set;}

        public List<ComicPageModel> ComicPages { get; set; }

        public int NumberOfPages {get;set;}

        public ComicBookViewModel(ComicBookModel comic)
        {
            Title = comic.Title;
            WebcomicId = comic.EntityId;
            Author = comic.Author;
            Genre = comic.Genre;
            EditionNumber = comic.EditionNumber;
            Description = comic.Description;
            ComicPages = (List<ComicPageModel>)comic.ComicPages;
            if (ComicPages == null)
            {
                NumberOfPages = 0;
            }
            else { NumberOfPages = ComicPages.Count; }
        }
    }
}

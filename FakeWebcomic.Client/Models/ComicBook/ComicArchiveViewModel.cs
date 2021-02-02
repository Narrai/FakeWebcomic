using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class ComicArchiveViewModel
    {
        public ComicBookModel ComicBook {get;set;}

        public ComicArchiveViewModel(ComicBookModel comicbook)
        {
            ComicBook = comicbook;
        }
    }
}

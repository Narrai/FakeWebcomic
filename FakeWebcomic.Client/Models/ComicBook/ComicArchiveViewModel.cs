using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class MainArchiveViewModel
    {
        public ComicBookModel ComicBook {get;set;}

        public ComicArchiveViewModel(ComicBookModel comicbook)
        {
            ComicBook = comicbook;
        }
    }
}

using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class MainArchiveViewModel
    {
        public List<ComicBookModel> ComicBooks {get;set;}

        public MainArchiveViewModel(List<ComicBookModel> list)
        {
            ComicBooks = list;
        }
    }
}
using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class AuthorHomeViewModel
    {
        public string Name {get;set;}
        public List<ComicBookModel> ComicBooks {get;set;}
    }
}
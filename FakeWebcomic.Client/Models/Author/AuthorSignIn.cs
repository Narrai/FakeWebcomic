using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class AuthorSignInViewModel
    {
        public List<Author> Authors {get;set;}

        public ComicArchiveViewModel(List<Author> authors)
        {
            Authors = authors;
        }
    }
}
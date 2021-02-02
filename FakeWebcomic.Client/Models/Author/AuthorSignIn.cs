using System.Collections.Generic;

namespace FakeWebcomic.Client.Models
{
    public class AuthorSignInViewModel
    {
        public List<Author> Authors {get;set;}

        public AuthorSignInViewModel(List<Author> authors)
        {
            Authors = authors;
        }
    }
}
namespace FakeWebcomic.Client.Models
{
    public class MainAboutViewModel
    {
        public int NumberOfComicBooks {get;set;}
        public int NumberOfPages {get;set;}

        public MainAboutViewModel(int numberofcomicbooks, int numberofpages)
        {
            NumberOfComicBooks = numberofcomicbooks;
            NumberOfPages = numberofpages;
        }
    }
}

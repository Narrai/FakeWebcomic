using FakeWebcomic.Client.Helper;
using System;
using System.Drawing;
using System.Linq;

namespace FakeWebcomic.Client.Models
{
    public class ComicPageViewModel : AEntity
    {
        public string PageTitle {get;set;}
        public int PageNumber { get; set; }
        public long WebcomicId {get;set;}

        public string ComicTitle {get;set;}
        public Image Image {get;set;}
        public int FirstPageNumber {get;set;}
        public int PreviousPageNumber {get;set;}
        public int NextPageNumber {get;set;}
        public int RandomPageNumber {get;set;}

        //In case the author is getting fancy by skipping page numbers or something, we can't
        //just get the next and previous page numbers by adding/subtracting 1 from PageNumber;
        //we have to actually look up the page number of the next/previous page.
        //First page number, too, for that matter.
        private List<ComicPageModel> _allPages {get;set;}
        private int _currentIndex {get;set;}
        private int _randIndex {get;set;}

        public ComicViewModel(ComicPageModel comic)
        {
            PageTitle = comic.PageTitle;
            PageNumber = comic.PageNumber;
            ComicTitle = comic.ComicBook.Title;
            WebcomicId = comic.ComicBookId;
            Image = ImageConvertor.ConvertByteArrayToImage(comic.Image);

            _allPages = comic.ComicBook.ComicPages;
            _currentIndex = _allPages.FirstOrDefault(c => c == comic).IndexOf();

            FirstPageNumber = _allPages[0].PageNumber;
            //We'll check in the Controller to make sure there's at least one comic; if not, the user
            //will be directed to the About page and will never have the opportunity to click this
            //dead link.

            if (_currentIndex < (_allPages.Length - 1))
            {
                NextPageNumber = _allPages[_currentIndex + 1].PageNumber;
            }
            else {NextPageNumber = 0;}    //Shouldn't be possible, but sends to default page (Latest)

            if (_currentIndex < 1)
            {
                PreviousPageNumber = _allPages[_currentIndex - 1].PageNumber;
            }
            else {PreviousPageNumber = 0;}    //Shouldn't be possible, but sends to default page (Latest)

            _randIndex = (new Random()).Next(0, (comic.ComicBook.ComicPages.Length - 1));
            RandomPageNumber = _allPages[_randIndex].PageNumber;
        }
    }
}

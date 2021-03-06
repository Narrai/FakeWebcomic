using FakeWebcomic.Client.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;

namespace FakeWebcomic.Client.Models
{
    public class ComicPageViewModel : AEntity
    {
        [Required]
        public string PageTitle {get;set;}
        [Required]
        public int PageNumber { get; set; }
        public long WebcomicId {get;set;}
        public ComicBookModel ComicBook {get;set;}

        public string ComicTitle {get;set;}
        [Required]
        public Image Image {get;set;}
        public int FirstPageNumber {get;set;}
        public int PreviousPageNumber {get;set;}
        public int NextPageNumber {get;set;}
        public int RandomPageNumber {get;set;}
        public string AltText {get;set;}

        private List<ComicPageModel> _allPages {get;set;}
        private int _currentIndex {get;set;}
        private int _randIndex {get;set;}

        public ComicPageViewModel(ComicPageModel comic)
        {
            PageTitle = comic.PageTitle;
            PageNumber = comic.PageNumber;
            ComicBook = comic.ComicBook;
            ComicTitle = ComicBook.Title;
            WebcomicId = comic.ComicBookId;
            Image = ImageConvertor.ConvertByteArrayToImage(comic.Image);
            AltText = $"{PageNumber}. {PageTitle}";

            //In case the author is getting fancy by skipping page numbers or something, we can't
            //just get the next and previous page numbers by adding/subtracting 1 from PageNumber;
            //we have to actually look up the page number of the next/previous page.
            //First page number, too, for that matter.
            _allPages = (List<ComicPageModel>)ComicBook.ComicPages;
            int _currentIndex = _allPages.IndexOf(comic);

            FirstPageNumber = _allPages[0].PageNumber;
            //We'll check in the Controller to make sure there's at least one comic; if not, the user
            //will be directed to the About page and will never have the opportunity to click this 
            //dead link.

            if (_currentIndex < (_allPages.Count - 1))
            {
                NextPageNumber = _allPages[_currentIndex + 1].PageNumber;
            }
            else {NextPageNumber = 0;}    //sends to default page (Latest)

            if (_currentIndex > 0)
            {
                PreviousPageNumber = _allPages[_currentIndex - 1].PageNumber;
            }
            else {PreviousPageNumber = _allPages[0].PageNumber;}    //sends to first page

            _randIndex = (new Random()).Next(0, (ComicBook.ComicPages.Count - 1));
            RandomPageNumber = _allPages[_randIndex].PageNumber;
        }
    }
}

using FakeWebcomic.Client.Helper;
using System.Drawing;

namespace FakeWebcomic.Client.Models
{
    public class ComicViewModel
    {
        public string PageTitle {get;set;}
        public int PageNumber { get; set; }

        public string ComicTitle {get;set;}
        public Image Image {get;set;}

        public ComicViewModel(ComicPageModel comic)
        {
            PageTitle = comic.PageTitle;
            PageNumber = comic.PageNumber;
            ComicTitle = comic.ComicBook.Title;
            Image = ImageConvertor.ConvertByteArrayToImage(comic.Image);
        }
    }
}

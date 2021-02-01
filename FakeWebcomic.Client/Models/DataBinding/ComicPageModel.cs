using FakeWebcomic.Client.Helper;

namespace FakeWebcomic.Client.Models
{
    public class ComicPageModel : AEntity
    {
        public string PageTitle {get;set;}
        public int PageNumber { get; set; }
        public byte[] Image { get; set; }


        public long ComicBookId { get; set; }
        public ComicBookModel ComicBook { get; set; }

        public ComicPageModel() {}

        public ComicPageModel(ComicPageViewModel model)
        {
            PageTitle = model.PageTitle;
            PageNumber = model.PageNumber;
            Image = ImageConvertor.ConvertImageToByteArray(model.Image);
            ComicBookId = model.WebcomicId;
            ComicBook = model.ComicBook;
        }
    }
}

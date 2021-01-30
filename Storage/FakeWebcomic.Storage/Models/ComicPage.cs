
namespace FakeWebcomic.Storage.Models
{
    public class ComicPage : AEntity
    {
        public int PageNumber { get; set; }
        public byte[] Image { get; set; }


        public long ComicBookId { get; set; }
        public ComicBook ComicBook { get; set; }
    }
}

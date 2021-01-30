using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeWebcomic.Storage.Models
{
    [ApiController]
    [Route("[controller]")]
    public class ComicBookController : ControllerBase
    {
        private FakeWebcomicContext _ctx = new FakeWebcomicContext();

        [HttpGet]
        public async Task<IActionResult> GetComicBooks()
        {
            var comicBooks = _ctx.ComicBooks.Include(b => b.ComicPages);
            return await Task.FromResult(Ok(comicBooks));
        }

        [HttpPost]
        public async Task<IActionResult> AddComicBook(ComicBook comicBook)
        {
            _ctx.ComicBooks.Add(comicBook);
            _ctx.SaveChanges();
            return await Task.FromResult(Ok("Comic book was added!"));
        }

        // TOOD: Remove a comic book
        [HttpDelete]
        public async Task<IActionResult> RemoveComicBook(string title)
        {
            // Console.WriteLine("Parameter title: " + title);
            // Console.WriteLine("Ctx title: " + _ctx.ComicBooks.Last().Title);
            // Console.WriteLine(_ctx.ComicBooks.Where(c => c.Title == title).Last());
            //_ctx.ComicBooks.Remove(_ctx.ComicBooks.Where(c => c.Title == title).SingleOrDefault());
            //_ctx.SaveChanges();
            return await Task.FromResult(Ok("Comic book was removed!"));
        }
    }
}

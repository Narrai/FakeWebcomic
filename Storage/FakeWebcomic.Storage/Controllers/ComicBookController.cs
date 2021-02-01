using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeWebcomic.Storage.Models
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ComicBookController : ControllerBase
    {
        private readonly FakeWebcomicRepository _ctx;
        public ComicBookController(FakeWebcomicRepository context)
        {
            _ctx = context;
        }

        // GET api/comicbook
        [HttpGet]
        public async Task<IActionResult> GetComicBooks()
        {
            var comicBooks = _ctx.GetComicBooks(); //.Include(b => b.ComicPages);
            return await Task.FromResult(Ok(comicBooks));
        }

        // POST api/comicbook
        [HttpPost]
        public async Task<IActionResult> AddComicBook(ComicBook comicBook)
        {
            _ctx.GetComicBooks().Add(comicBook);
            _ctx.Save();
            return await Task.FromResult(Ok("Comic book was added!"));
        }

        // DELETE api/comicbook/title
        [HttpDelete("{title}")]
        public async Task<IActionResult> RemoveComicBook(string title)
        {
            var comicbooks = _ctx.GetComicBooks();
            comicbooks.Remove(comicbooks.Where(c => c.Title == title).SingleOrDefault());
            _ctx.Save();
            return await Task.FromResult(Ok("Comic book was removed!"));
        }
    }
}

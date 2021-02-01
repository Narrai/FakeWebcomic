using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeWebcomic.Storage.Models
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ComicPageController : ControllerBase
    {
        private readonly FakeWebcomicRepository _ctx;
        public ComicPageController(FakeWebcomicRepository context)
        {
            _ctx = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetComicBooks()
        {
            var comicPages = _ctx.GetComicPages().Include(c => c.ComicBook);
            return await Task.FromResult(Ok(comicPages));
        }

        // TODO: Get First Comic Page -> Input: Comic Book

        // TODO: Get Last Comic Page

        // TODO: Randomize Comic Page

        // TODO: Add Pages

        // TODO: Remove Page

    }
}

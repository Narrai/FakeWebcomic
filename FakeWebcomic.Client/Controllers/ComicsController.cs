using Microsoft.AspNetCore.Mvc;

namespace FakeWebcomic.Client.Controllers
{
    public class ComicsController
    {
        private FakeWebcomicRepository _repo;

        public ComicsController(FakeWebcomicRepository context)
        {
            _repo = context;
        }

        //View comic page; requires no authorization or validation
        [HttpGet]
        public IActionResult Get(int comicnumber)
        {
            if (comicnumber == 1)
            {
                return View("FirstComic",new ComicViewModel(_repo.Comics.FirstOrDefault(c => c.Number == 1)));
            }
            if (_repo.Comics.FirstOrDefault(c => c.Number == comicnumber) != null)
            {
                return View("MiddleComic",new ComicViewModel(_repo.Comics.FirstOrDefault(c => c.Number == comicnumber)));
            }
            return View("LastComic",new ComicViewModel(_repo.Comics.Last()));
        }

        //Alternatively, make separate actions for each link, which is probably not a bad idea.
        //That would let us determine the next comic by going to the next item in _repo.Comics rather
        //than assuming that the next number will reference a real comic.
    }
}

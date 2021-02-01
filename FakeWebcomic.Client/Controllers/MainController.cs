using FakeWebcomic.MvcClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FakeWebcomic.Client.Controllers
{
    public class ComicsController
    {
        private string _storageApi = "https://localhost:6002/comicbook";
        private HttpClient _http = new HttpClient();

        //Archive
        [HttpGet]
        public async IActionResult GetArchive(long WebcomicId)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                ComicBooks.OrderBy(c => c.Title);
                return View ("MainArchive",new MainArchiveViewModel(ComicBooks));
            }
        }

        //About
        [HttpGet]
        public IActionResult GetAbout(long WebcomicId)
        {
            return View("MainAbout");
        }

        //Add webcomic
        [HttpGet]
        public IActionResult GetPostWebcomic()
        {
            return View("PostWebcomic", new ComicAboutViewModel(new ComicBookModel()));
        }
        [HttpPost]
        public async IActionResult PostWebcomic(ComicBookModel model)
        {
            if (ModelState.IsValid)
            {
                var content = ComicPageModel(model)
                var response = await _http.PostAsync(_storageApiPage,content);
                if (response.IsSuccessCode)
                {
                    return View("SuccessfulNewPage", page);
                }
                else 
                {
                    return View("FailedNewPage", page);
                }
            }
            return View("FailedNewPage", page);

        }
    }
}

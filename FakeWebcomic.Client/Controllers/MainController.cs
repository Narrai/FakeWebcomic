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
                return View ("MainArchive",new MainArchiveViewModel(ComicBooks));
            }
        }

        //About
        [HttpGet]
        public async IActionResult GetAbout(long WebcomicId)
        {
            return View("MainAbout");
        }
    }
}

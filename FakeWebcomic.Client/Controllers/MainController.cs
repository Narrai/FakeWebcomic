using FakeWebcomic.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FakeWebcomic.Client.Controllers
{
    [Route("[controller]/[action]")]
    public class MainController : Controller
    {
        private string _webcomicsUri = "https://localhost:5001/api/comicbook";
        private HttpClientHandler _clientHandler = new HttpClientHandler();

        //Archive
        [HttpGet]
        public async Task<IActionResult> Archive()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
            {
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                    ComicBooks.OrderBy(c => c.Title);
                    return View("MainArchiveView", new MainArchiveViewModel(ComicBooks));
                }
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //About
        [HttpGet]
        public async Task<IActionResult> About()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
            {
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                    int numberofpages = 0;
                    foreach (var webcomic in ComicBooks)
                    {
                        numberofpages += webcomic.ComicPages.Count;
                    }
                    return View("MainAboutView", new MainAboutViewModel(ComicBooks.Count(), numberofpages));
                }
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //Add webcomic
        [HttpGet]
        [Authorize]
        public IActionResult GetPostWebcomic()
        {
            ComicBookModel webcomic = new ComicBookModel()
            {
                Author = User.Identity.Name
            };
            return View("PostWebcomicView", new ComicBookViewModel(webcomic));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostWebcomic(ComicBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var _http = new HttpClient(_clientHandler))
                {
                    var stringData = JsonConvert.SerializeObject(new ComicBookModel(model));
                    var stringContent = new StringContent(stringData, UnicodeEncoding.UTF8, "application/json");
                    var response = await _http.PostAsync(_webcomicsUri, stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return View("SuccessfulNewPageView");
                    }
                    return View("FailedNewPageView");
                }
            }
            return View("GetPostWebcomicView", model);
        }
    }
}

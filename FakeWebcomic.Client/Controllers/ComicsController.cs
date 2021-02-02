using FakeWebcomic.Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FakeWebcomic.Client.Controllers
{
    [Route("[controller]/[action]")]

    public class ComicsController : Controller
    {
        private string _webcomicsUri = "https://localhost:5001/api/comicbook";
        private string _pagesUri = "https://localhost:5001/api/comicpage";
        private HttpClientHandler _clientHandler = new HttpClientHandler();

        //View comic page; requires no authorization or validation
        [HttpGet]
        public async Task<IActionResult> GetPage(string WebcomicName,int PageNumber)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
			{
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());

                    if (ComicBooks.FirstOrDefault(c => c.Title == WebcomicName) != null)
                    {
                        ComicBookViewModel webcomic = new ComicBookViewModel(ComicBooks.FirstOrDefault(c => c.Title == WebcomicName));

                        webcomic.ComicPages.OrderBy(p => p.PageNumber);

                        //First, if there aren't any pages, you get sent to the About page instead of the
                        //latest page.
                        if (webcomic.ComicPages.Count == 0)
                        {
                            return await (new ComicsController()).GetAbout(WebcomicName);
                        }

                        if (webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber) != null)
                        {
                            ComicPageModel page = webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber);
                            ComicPageViewModel pageview = new ComicPageViewModel(page);
                            if (PageNumber == pageview.FirstPageNumber)
                            {
                                return View("FirstPageView",pageview);
                            }
                            if (PageNumber == 0 || PageNumber == webcomic.ComicPages[^1].PageNumber)
                            // PageNumber == 0 is the designation for default page (Latest)
                            {
                                return View("LatestPageView",pageview);
                            }
                            return View("MiddlePageView",pageview);
                        }
                        return View("LatestPageView",new ComicPageViewModel(webcomic.ComicPages[^1]));
                        //If the page number is invalid, send to the default page (Latest).
                        //Should be impossible via any in-application links.
                    }
                    return await (new MainController()).Archive();
                    //If the webcomic doesn't exist, kick them back to the archive of webcomics.
                    //Should be impossible via any in-application links.
                }
                return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //Archive
        [HttpGet]
        public async Task<IActionResult> GetArchive(string WebcomicName)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
			{
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());

                    if (ComicBooks.FirstOrDefault(c => c.Title == WebcomicName) != null)
                    {
                        ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title == WebcomicName);
                        webcomic.ComicPages.OrderBy(p => p.PageNumber);
                        return View("ComicArchiveView",new ComicArchiveViewModel(webcomic));
                    }
                    return await (new MainController()).Archive();
                    //If the webcomic doesn't exist, kick them back to the archive of webcomics.
                    //Should be impossible via any in-application links.
                }
            }
            return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //About
        [HttpGet]
        public async Task<IActionResult> GetAbout(string WebcomicName)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
			{
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());

                    if (ComicBooks.FirstOrDefault(c => c.Title == WebcomicName) != null)
                    {
                        ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title == WebcomicName);
                        webcomic.ComicPages.OrderBy(p => p.PageNumber);
                        return View("ComicAboutView",new ComicBookViewModel(webcomic));
                    }
                    return await (new MainController()).Archive();
                    //If the webcomic doesn't exist, you get kicked back to the main archive.
                }
                return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //Create new page
        [HttpGet]
        public async Task<IActionResult> GetNewPage(string WebcomicName)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
			{
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());

                    if (ComicBooks.FirstOrDefault(c => c.Title == WebcomicName) != null)
                    {
                        ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title == WebcomicName);

                        if (User.Identity.Name != webcomic.Author)
                        {
                            //Impossible via in-application links; user is hacking. Back to main page,
                            //for lack of a more severe punishment.
                            return View("MainArchiveView", new MainArchiveViewModel(ComicBooks));
                        }

                        webcomic.ComicPages.OrderBy(p => p.PageNumber);
                        ComicPageModel page = new ComicPageModel(){
                            ComicBookId = webcomic.EntityId,
                            ComicBook = webcomic
                        };
                        return View("NewPageView",new ComicPageViewModel(page));
                    }
                    return await (new MainController()).Archive();
                    //If the webcomic doesn't exist, kick them back to the archive of webcomics.
                    //Should be impossible via any in-application links.
                }
                return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //still posting new page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostPage(ComicPageViewModel page)
        {
            if (ModelState.IsValid)
            {
                _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var _http = new HttpClient(_clientHandler))
                {
                    var stringData = JsonConvert.SerializeObject(new ComicPageModel(page));
                    var stringContent = new StringContent(stringData, UnicodeEncoding.UTF8, "application/json");
                    var response = await _http.PostAsync(_pagesUri,stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return View("SuccessfulNewPageView", page);
                    }
                    return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
            }
            return View("FailedNewPageView", page);
        }

        //Modify old page
        [HttpGet]
        public async Task<IActionResult> GetUpdatePage(string WebcomicName,int PageNumber)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
			{
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());

                    if (ComicBooks.FirstOrDefault(c => c.Title == WebcomicName) != null)
                    {
                        ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title == WebcomicName);
                        webcomic.ComicPages.OrderBy(p => p.PageNumber);

                        if (User.Identity.Name != webcomic.Author)
                        {
                            //Impossible via in-application links; user is hacking. Back to main page,
                            //for lack of a more severe punishment.
                            return View("MainArchiveView", new MainArchiveViewModel(ComicBooks));
                        }

                        if (webcomic.ComicPages.Count == 0)
                        {
                            return View("ComicArchiveView", new ComicArchiveViewModel(webcomic));
                            //There is no page to update; back to the archive with you
                        }

                        if (webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber) != null)
                        {
                            ComicPageModel page = webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber);
                            ComicPageViewModel pageview = new ComicPageViewModel(page);
                            return View("UpdatePageView",pageview);
                        }
                        return View("ComicArchiveView", new ComicArchiveViewModel(webcomic));
                        //There is no page to update; back to the archive with you
                    }
                    return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
            }
            return await (new MainController()).Archive();
            //If the webcomic doesn't exist, you get kicked back to the main archive.
            //Should be impossible via in-app links.
        }

        //still updating old page
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePage(ComicPageViewModel page)
        {
            if (ModelState.IsValid)
            {
                _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var _http = new HttpClient(_clientHandler))
                {
                    var stringData = JsonConvert.SerializeObject(new ComicPageModel(page));
                    var stringContent = new StringContent(stringData, UnicodeEncoding.UTF8, "application/json");
                    var response = await _http.PutAsync(_pagesUri,stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return View("SuccessfulNewPageView", page);
                    }
                    return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
            }
            return View("FailedNewPageView", page);
        }

        //Delete a page
        [HttpDelete]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePage(string WebcomicName,int PageNumber, ComicArchiveViewModel model)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
			{
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                    if (ComicBooks.FirstOrDefault(c => c.Title == WebcomicName) != null)
                    {
                        ComicBookViewModel webcomic = new ComicBookViewModel(ComicBooks.FirstOrDefault(c => c.Title == WebcomicName));

                        if (User.Identity.Name != webcomic.Author)
                        {
                            //Impossible via in-application links; user is hacking. Back to main page,
                            //for lack of a more severe punishment.
                            return View("MainArchiveView", new MainArchiveViewModel(ComicBooks));
                        }

                        if (webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber) != null)
                        {
                            ComicPageModel page = webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber);
                            var stringData = JsonConvert.SerializeObject(page);
                            var stringContent = new StringContent(stringData, UnicodeEncoding.UTF8, "application/json");
                            var request = new HttpRequestMessage {
                                Method = HttpMethod.Delete,
                                RequestUri = new Uri(_pagesUri),
                                Content = stringContent
                            };
                            var response2 = await _http.SendAsync(request);
                            if (response2.IsSuccessStatusCode)
                            {
                                //switch to AuthorController.AuthorHome once that's up and running
                                return View("ComicArchiveView", new ComicArchiveViewModel(new ComicBookModel(webcomic)));
                            }
                            return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                        }
                        return View("ComicArchiveView", model);
                        //There is no page to delete; back to the archive with you!
                        //switch to AuthorController.AuthorHome once that's up and running
                    }
                    return View("MainArchiveView", new MainArchiveViewModel(ComicBooks));
                    //There is no such webcomic; back to the archive with you!
                    //Impossible via in-application links.
                }
            }
            return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Udpate About page (and by extension the entire webcomic object)
        [HttpGet]
        public async Task<IActionResult> GetUpdateAbout(string WebcomicName)
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var _http = new HttpClient(_clientHandler))
			{
                var response = await _http.GetAsync(_webcomicsUri);
                if (response.IsSuccessStatusCode)
                {
                    var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());

                    if (ComicBooks.FirstOrDefault(c => c.Title == WebcomicName) != null)
                    {
                        ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title == WebcomicName);
                        webcomic.ComicPages.OrderBy(p => p.PageNumber);
                        return View("UpdateAboutView", new ComicBookViewModel(webcomic));
                    }
                    return await (new MainController()).Archive();
                    //If the webcomic doesn't exist, back to the main archive
                }
                return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        //still updating about/webcomic
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAbout(ComicBookViewModel model)
        {
            if (ModelState.IsValid)
            {
                _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                using (var _http = new HttpClient(_clientHandler))
                {
                    var stringData = JsonConvert.SerializeObject(new ComicBookModel(model));
                    var stringContent = new StringContent(stringData, UnicodeEncoding.UTF8, "application/json");
                    var response = await _http.PutAsync(_webcomicsUri,stringContent);
                    if (response.IsSuccessStatusCode)
                    {
                        return View("SuccessfulNewPageView", model);
                    }
                }
                return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            return View("FailedNewPageView", model);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FakeWebcomic.Client.Controllers
{

    public class ComicsController
    {
        private string _storageApi = "https://localhost:6002/comicbook";
        private string _storageApiPage = "https://localhost:6002/comicpage";
        private HttpClient _http = new HttpClient();

        //View comic page; requires no authorization or validation
        [HttpGet]
        public async IActionResult GetPage(string WebcomicName,int PageNumber)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.Title == WebcomicName))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title = WebcomicName);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);
                    
                    //First, if there aren't any pages, you get sent to the About page instead of the
                    //latest page.
                    if (webcomic.ComicPages.Length == 0)
                    {
                        return (new ComicsController()).GetAbout(WebcomicName);
                    }

                    if (webcomic.ComicPages.Contains(p => p.PageNumber == PageNumber))
                    {
                        ComicPageModel page = webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber);
                        ComicPageViewModel pageview = new ComicPageViewModel(page);
                        if (PageNumber == pageview.FirstPageNumber)
                        {
                            return View("FirstPage",pageview);
                        }
                        if (PageNumber == 0 || PageNumber == webcomic.ComicPages[^1].PageNumber)    
                        // PageNumber == 0 is the designation for default page (Latest)
                        {
                            return View("LatestPage",pageview);
                        }
                        return View("MiddlePage",pageview);
                    }
                    return View("LatestPage",new ComicViewModel(webcomic.ComicPages[^1]));
                    //If the page number is invalid, send to the default page (Latest).
                    //Should be impossible via any in-application links.
                }

                else {
                    return (new MainController()).Archive();
                    //If the webcomic doesn't exist, kick them back to the archive of webcomics.
                    //Should be impossible via any in-application links.
                }
            }
            else {
                return(new MainController()).Archive();
                //If the response fails, you get kicked back to the main archive.
            }
        }

        //Archive
        [HttpGet]
        public async IActionResult GetArchive(string WebcomicName)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.Title == WebcomicName))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title = WebcomicName);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);
                    return View("ComicArchive",ComicArchiveViewModel(webcomic));
                }
                else 
                {
                    return (new MainController()).Archive();
                    //If the webcomic doesn't exist, kick them back to the archive of webcomics.
                    //Should be impossible via any in-application links.
                }
            }
            else 
            {
                return (new MainController()).Archive();
                //If the response fails, you get kicked back to the main archive.
            }
        }

        //About
        [HttpGet]
        public async IActionResult GetAbout(string WebcomicName)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.Title == WebcomicName))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title = WebcomicName);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);
                    return View("ComicAbout",ComicAboutViewModel(webcomic));
                }
            }
            return (new MainController()).Archive();
            //If the response fails or the webcomic doesn't exist, you get kicked back to the main archive.
            }
        }

        //Create new page
        [HttpGet]
        public async IActionResult GetNewPage(string WebcomicName)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.Title == WebcomicName))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title = WebcomicName);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);
                    ComicPageModel page = new ComicPageModel{
                        ComicBookId = webcomic.EntityId,
                        ComicBook = webcomic
                    }
                    return View("NewPage",ComicPageViewModel(page));
                }
                else
                {
                    return (new MainController()).Archive();
                    //If the webcomic doesn't exist, kick them back to the archive of webcomics.
                    //Should be impossible via any in-application links.
                }
            }
            else
            {
                return (new MainController()).Archive();
                //If the response fails, you get kicked back to the main archive.
            }
        }
        [HttpPost]
        public async IActionResult PostPage(ComicPageViewModel page)
        {
            if (ModelState.IsValid)
            {
                var content = ComicPageModel(page)
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

        //Modify old page
        [HttpGet]
        public async IActionResult GetUpdatePage(string WebcomicName,int PageNumber)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.Title == WebcomicName))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title = WebcomicName);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);

                    if (webcomic.ComicPages.Length == 0)
                    {
                        return (new AuthorController()).Home(webcomic.Auhthor);
                        //There is no page to update; back to AuthorHome with you
                    }

                    if (webcomic.ComicPages.Contains(p => p.PageNumber == PageNumber))
                    {
                        ComicPageModel page = webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber);
                        ComicPageViewModel pageview = new ComicPageViewModel(page);
                        return View("NewPage",pageview);
                    }
                    return (new AuthorController()).Home(webcomic.Auhthor);
                    //There is no page to update; back to AuthorHome with you
                }
            }
            return (new MainController()).Archive();
            //If the response fails or the webcomic doesn't exist, you get kicked back to the main archive.
            //Should be impossible via in-app links.
        }
        [HttpUpdate]
        public async IActionResult UpdatePage(string WebcomicName, int PageNumber)
        {
            if (ModelState.IsValid)
            {
                var content = ComicPageModel(page)
                var response = await _http.UpdateAsync(_storageApiPage,content);
                if (response.IsSuccessCode)
                {
                    return View("SuccessfulNewPage", page);
                }
            }
            return View("FailedNewPage", page);
        }

        //Delete a page
        [HttpDelete]
        public async IActionResult DeletePage(string WebcomicName,int PageNumber)
        {
            
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.Title == WebcomicName))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title = WebcomicName);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);

                    if (webcomic.ComicPages.Length == 0)
                    {
                        return (new AuthorController()).Home(webcomic.Auhthor);
                        //There is no page to delete; back to AuthorHome with you
                    }

                    if (webcomic.ComicPages.Contains(p => p.PageNumber == PageNumber))
                    {
                        var content = webcomic.ComicPages.FirstOrDefault(p => p.PageNumber == PageNumber);
                        var response = await _http.DeleteAsync(_storageApiPage,content);
                        if (response.IsSuccessCode)
                        {
                            return View("SuccessfulNewPage", page);
                        }
                    }
                    return (new AuthorController()).Home(webcomic.Auhthor);
                    //There is no page to delete; back to AuthorHome with you                    
                }
            }
            return (new MainController()).Archive();
            //If the response fails or the webcomic doesn't exist, you get kicked back to the main archive.
            //Should be impossible via in-app links.
        }

        //Udpate About page (and by extension the entire webcomic object)
        [HttpGet]
        public async IActionResult GetUpdateAbout(string WebcomicName)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.Title == WebcomicName))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.Title = WebcomicName);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);
                    return View("UpdateAbout",ComicAboutViewModel(webcomic));
                }
            }
            return (new MainController()).Archive();
            //If the response fails or the webcomic doesn't exist, you get kicked back to the main archive.
            }
        }

        [HttpUpdate]
        public async IActionResult UpdateAbout(ComicAboutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var content = ComicBookModel(model)
                var response = await _http.UpdateAsync(_storageApi,content);
                if (response.IsSuccessCode)
                {
                    return View("SuccessfulNewPage", model);
                }
            }
            return View("FailedNewPage", model);
        }
    }
}

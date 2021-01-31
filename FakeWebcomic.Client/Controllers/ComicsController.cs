using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FakeWebcomic.Client.Controllers
{

    public class ComicsController
    {
        private string _storageApi = "https://localhost:6002/comicbook";
        private HttpClient _http = new HttpClient();

        //View comic page; requires no authorization or validation
        [HttpGet]
        public async IActionResult GetPage(long WebcomicId,int PageNumber)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.EntityId == WebcomicId))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.EntityId = WebcomicId);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);
                    
                    //First, if there aren't any pages, you get sent to the About page instead of the
                    //latest page.
                    if (webcomic.ComicPages.Length == 0)
                    {
                        return (new AboutController()).Get(webcomic);
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
        public async IActionResult GetArchive(long WebcomicId)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.EntityId == WebcomicId))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.EntityId = WebcomicId);
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
        public async IActionResult GetAbout(long WebcomicId)
        {
            var response = await _http.GetAsync(_storageApi);
            if (response.IsSuccessCode)
            {
                var ComicBooks = JsonConvert.DeserializeObject<List<ComicBookModel>>(await response.Content.ReadAsStringAsync());
                
                if (ComicBooks.Contains(c => c.EntityId == WebcomicId))
                {
                    ComicBookModel webcomic = ComicBooks.FirstOrDefault(c => c.EntityId = WebcomicId);
                    webcomic.ComicPages.OrderBy(p => p.PageNumber);
                    return View("ComicAbout",ComicAboutViewModel(webcomic));
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
    }
}

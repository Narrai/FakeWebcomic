using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FakeWebcomic.Client.Models;
using System.Net.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace FakeWebcomic.Client.Controllers
{
    [Route("[controller]")]
    public class ArchiveController : Controller
    {
        private HttpClient _http = new HttpClient();

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _http.GetAsync("https://localhost:6002/user");
            var content = JsonConvert.DeserializeObject<ArchiveViewModel>(await response.Content.ReadAsStringAsync());
            return View("home", content);
        }
    }
}

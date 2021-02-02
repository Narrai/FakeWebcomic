using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace FakeWebcomic.Client.Controllers
{
    [Route("[controller]")]
    public class AuthorController : Controller
    {
        [HttpGet]
        [Authorize]
        public async Task<IACtionResult> AuthorHome()
        {
            //search for author object in api based on User.Identity.Name
            //Alternatively, retrieve all webcomics with Author == User.Identity.Name
        }

        [HttpPost]
        public async Task<IACtionResult> NewAuthor()
        {
            //add account to Okto, add authour to Storage API
            return await AuthorHome();
        }
    }
}

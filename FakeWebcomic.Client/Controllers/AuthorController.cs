using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FakeWebcomic.Client.Controllers
{
    [Route("[controller]")]
    public class AuthorController : Controller
    {
        // [HttpGet]
        // [Authorize]
        // public async Task<IActionResult> AuthorHome()
        // {
        //     //search for author object in api based on User.Identity.Name
        //     //Alternatively, retrieve all webcomics with Author == User.Identity.Name
        //     return await Task<IActionResult>();
        // }

        // [HttpPost]
        // public async Task<IActionResult> NewAuthor()
        // {
        //     //add account to Okto, add authour to Storage API
        //     return await AuthorHome();
        // }
    }
}

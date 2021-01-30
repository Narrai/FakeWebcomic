using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeWebcomic.Storage.Models
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private FakeWebcomicContext _ctx = new FakeWebcomicContext();

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var user = _ctx.Users;
            return await Task.FromResult(Ok(user));
        }

        // TODO: Get Admin

        // TODO: Create Users
    }
}

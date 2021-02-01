using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FakeWebcomic.Storage.Models
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly FakeWebcomicRepository _ctx;

        public UserController(FakeWebcomicRepository context)
        {
            _ctx = context;
        }

        // Get List of Users
        // api/user
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = _ctx.GetUsers();
            return await Task.FromResult(Ok(users));
        }

        //  Create Users
        // api/user/{insert name here}
        [HttpPost("{name}")]
        public async Task<IActionResult> CreateUser(string name)
        {
            _ctx.AddUser(name);
            return await Task.FromResult(Ok("Created User"));
        }

        // Get Admin
        // api/user/admin
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdmin()
        {
            var admin = _ctx.GetUsers().Where(u => u.IsAdmin == true);
            return await Task.FromResult(Ok(admin));
        }
    }
}

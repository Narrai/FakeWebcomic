using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FakeWebcomic.Storage.Models
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private FakeWebcomicContext _ctx = new FakeWebcomicContext();

        [HttpGet]
        public async Task<IActionResult> GetTask()
        {
            var user = _ctx.Users.FirstOrDefault();
            return await Task.FromResult(Ok(user.Name));
        }
    }

}

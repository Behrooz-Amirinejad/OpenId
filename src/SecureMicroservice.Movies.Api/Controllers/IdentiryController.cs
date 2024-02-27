using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecureMicroservice.Movies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IdentiryController : ControllerBase
    {
        public IdentiryController()
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return await Task.FromResult(new JsonResult(from c in User.Claims select new { c.Type, c.Value }));
        }
    }
}

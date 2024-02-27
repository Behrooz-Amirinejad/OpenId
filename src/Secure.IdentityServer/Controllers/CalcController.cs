using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Secure.IdentityServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CalcController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Add() => (2 + 12).ToString();
    }
}

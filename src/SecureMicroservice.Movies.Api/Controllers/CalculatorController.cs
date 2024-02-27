using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SecureMicroservice.Movies.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CalculatorController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        [HttpGet]
        public async Task<string> GetAdd()
        {

            string baseUrl = _configuration.GetValue<string>("IdnetiyServerUrl");
            Console.WriteLine($"service URL: {baseUrl}");
            var handler = new IgnoreSslCertificateHandler();
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri(baseUrl);
            var val = await client.GetStringAsync("api/Calc/add");
            Console.Write(val);
            return val;



        }
    }
}

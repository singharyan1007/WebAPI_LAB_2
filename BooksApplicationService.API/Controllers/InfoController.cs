using BooksApplicationService.API.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BooksApplicationService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly IOptions<MySettings> _settings;

        public InfoController(IOptions<MySettings> settings)
        {
            _settings = settings;
        }

        [HttpGet]
        public IActionResult GetInfo() => Ok(new
        {
            AppName = _settings.Value.ApplicationName,
            Version = _settings.Value.Version
        });
    }
}

using BooksApplicationService.API.Model.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BooksApplicationService.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class GreetingsController : ControllerBase
    {
        private readonly IGreetingsService _greetingService;
        public GreetingsController(IGreetingsService greetingService)
        {
            _greetingService = greetingService;
        }


        [HttpGet("{name:string}")]
        public IActionResult Greet(string name)
        {
            var greeting = _greetingService.Greet(name);
            return Ok(greeting);
        }
    }
}

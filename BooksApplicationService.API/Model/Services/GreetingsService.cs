namespace BooksApplicationService.API.Model.Services
{
    public class GreetingsService:IGreetingsService
    {
        public string Greet(string name)
        {
            return $"Hello, {name}!";
        }
    }
}

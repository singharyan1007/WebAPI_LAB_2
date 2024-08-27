using BooksApplicationService.API.Model.Entities;

namespace BooksApplicationService.API.Model.Interfaces
{
    public interface ITokenService
    {
         string GenerateToken(ApplicationUser user);
    }
}

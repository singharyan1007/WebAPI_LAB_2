using BooksApplicationService.API.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace BooksApplicationService.API.Model.Interfaces
{
    public interface ITokenService
    {
         Task<string> GenerateToken(IdentityUser user);
    }
}

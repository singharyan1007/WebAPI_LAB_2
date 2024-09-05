using BooksApplicationService.API.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BooksApplicationService.API.Model.Data
{
    public class BookDbContext:IdentityDbContext<IdentityUser>
    {

        public BookDbContext() { }


        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) { }


        public DbSet<Book> NewBooks { get; set; }
       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=BooksCatalogDb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

            //If configured has not been done then use this for being on the safer side
        }

    }
}

using BooksApplicationService.API.Model.Data;
using BooksApplicationService.API.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooksApplicationService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly BookDbContext _db;
        public BooksController(BookDbContext db) { _db = db; }


        //GET : .../api/books
        [HttpGet]
        [Authorize]

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _db.NewBooks.ToListAsync();
        }


        //POST : .../api/books/

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType<Book>(StatusCodes.Status201Created)]
        [ProducesResponseType<Book>(StatusCodes.Status400BadRequest)]
        
        public async Task<ActionResult> Add([FromBody]Book book)
        {
            //Validate
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.NewBooks.Add(book);
            await _db.SaveChangesAsync();
            return Created($"/api/books/{book.BookId}", value: book);
        }


        //PUT : .../api/books/{id}
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType<Book>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Book>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<Book>(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult> Update([FromQuery] int id, [FromBody] Book book)
        {
            var b=await _db.NewBooks.FindAsync(id);
            if (b == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            b.Name = book.Name;
            b.Description = book.Description;
            b.Author = book.Author;
            b.Price = book.Price;

            _db.SaveChanges();
            return Ok();
        }

        //DELETE : .../api/books/{id}
        [HttpDelete]
        [Authorize]
        [Consumes("application/json")]
        [ProducesResponseType<Book>(StatusCodes.Status404NotFound)]
        [ProducesResponseType<Book>(StatusCodes.Status200OK)]
        public async Task<ActionResult> Delete([FromQuery] int id)
        {
            var b = await _db.NewBooks.FindAsync(id);
            if (b == null)
            { 
            return NotFound();
            }
            _db.NewBooks.Remove(b);
            _db.SaveChanges();
            return Ok();
        }




    }
}

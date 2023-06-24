using BookShoppingCart.Data;
using BookShoppingCart.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCart.Ripository
{
    public class HomeRepository :IHomeRepository
    {
        private readonly ApplicationDbContext _context;
        public HomeRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int GenreId = 0)
        {
            sTerm=sTerm.ToLower();
            IEnumerable<Book> books = await (
                            from book in _context.Book
                            join genre in _context.Genre
                            on book.GenreId equals genre.Id
                            where string.IsNullOrWhiteSpace(sTerm) || book.BookName.ToLower().StartsWith(sTerm)
                            select new Book
                            {
                                Id=book.Id,
                                AuthorName=book.AuthorName,
                                 BookName=book.BookName,
                                GenreId=book.GenreId,
                                GenreName=genre.GenreName,
                            }
                        ).ToListAsync();
            if(GenreId > 0 )
            {
                books = books.Where(a=>a.GenreId==GenreId).ToList();
            }
            return books; 
        }



        public async Task<IEnumerable<Genre>> GenreList()
        {
            return await _context.Genre.ToListAsync();
        }
    }
}

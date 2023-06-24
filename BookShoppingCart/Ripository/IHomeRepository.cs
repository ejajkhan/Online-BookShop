using BookShoppingCart.Models;

namespace BookShoppingCart
{
    public interface IHomeRepository
    {
         Task<IEnumerable<Book>> GetBooks(string sTerm = "", int GenreId = 0);
         Task<IEnumerable<Genre>> GenreList();
    }
}
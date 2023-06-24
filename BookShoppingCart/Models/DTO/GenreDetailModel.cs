namespace BookShoppingCart.Models.DTO
{
    public class GenreDetailModel
    {
        public Genre Genre { get; set; }
        public IEnumerable<Book> Book { get; set; }
    }
}

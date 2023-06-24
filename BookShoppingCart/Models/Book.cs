using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BookShoppingCart.Models
{
    [Table("Book")]
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string BookName { get; set; }
        [Required]
        public string AuthorName { get; set; }
        [Required]
        public Double Price { get; set; }
        public string? BookImage { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<CartDetail>? CartDetails { get; set; }
        [NotMapped]
        public string? GenreName { get; set; }
    }
}

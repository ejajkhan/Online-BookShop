using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCart.Models
{
    public class Author
    {
        
        public int Id { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public string? Description { get; set; }
        public List<Book>? Books { get; set; }
    }
}

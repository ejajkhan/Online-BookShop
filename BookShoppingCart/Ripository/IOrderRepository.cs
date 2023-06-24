using BookShoppingCart.Models;

namespace BookShoppingCart
{
    public interface IOrderRepository
    {
        IEnumerable<Order> UserOrder();
    }
}
using BookShoppingCart.Data;
using BookShoppingCart.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCart.Ripository
{
    public class OrderRepository:IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartRepository _cartRepo;

        public OrderRepository(ApplicationDbContext application,ICartRepository cartRepository)
        {
            _context= application;
            _cartRepo= cartRepository;
        }


        public IEnumerable<Order> UserOrder()
        {
            var userId=_cartRepo.getUserId();
            if(userId == null) { throw new Exception("User not Logged in"); }
            var order=_context.Order
                            .Where(x => x.UserId == userId)
                            .Include(x=>x.OrderStatus)
                            .Include(x=>x.OrderDetails)
                            .ThenInclude(x=>x.Book)
                            .ThenInclude(x=>x.Genre)
                            .ToList();
            return order;
        }
















    }
}

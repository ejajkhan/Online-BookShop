using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCart.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _context;

        
        public OrderController(IOrderRepository orderRepository)
        {
            _context = orderRepository;
        }
        public IActionResult UserOrder()
        {
            var order = _context.UserOrder();
            return View(order);
        }
    }
}

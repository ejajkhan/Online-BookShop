using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCart.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _context;

        public CartController(ICartRepository cartRepository)
        {
            _context = cartRepository;
        }









        public async Task<IActionResult> AddItem(int bookId, int qty=1, int redirect=0)
        {
            var addConfirmation= _context.AddItem(bookId, qty);
            int Counter =  _context.GetCartItemCount();
            if (addConfirmation ==1 || addConfirmation== -1)
            {
                Console.WriteLine("Item Added SuccessFully");
                TempData["Success"] = "Item Added SuccessFully";
                if (redirect == 1) {  return RedirectToAction("Index","Home"); }
                if (redirect == 2) { return RedirectToAction("GetUserCart", "Cart"); }
                return RedirectToAction();
                //return Ok(Counter);

            }
            else
            {
                throw new Exception("Database Error");
            }
            
            

            
            
        }







        public async Task<IActionResult> RemoveItem(int bookId, int qty=1)
        {
            var cartCount =  _context.RemoveItem(bookId, qty);
            
            return RedirectToAction("GetUserCart");
        }


        public async Task<IActionResult> GetUserCart()
        {
            var cart=  _context.GetUserCart();
            int Amount = 0;
            if (cart != null)
            {
                foreach (var item in cart.CartDetails)
                {
                    Amount = Amount + (int)(item.Quantity * item.Book.Price);
                }
                ViewData["Total"] = Amount;

            }
            
            return View(cart);
        }

        public async  Task<IActionResult> GetTotalItemInCart() 
        {
            int CartCount =  _context.GetCartItemCount();
            return Ok(CartCount);
        }

        public async Task<IActionResult> CheckOut()
        {
            bool result = _context.DocheckOut();
             RedirectToAction();
            if(result==false) { throw new Exception("Something happended in database"); }
            return View("OrderCompilation");
            
        }
    }
}

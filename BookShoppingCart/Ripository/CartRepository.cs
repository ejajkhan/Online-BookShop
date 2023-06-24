using BookShoppingCart.Data;
using BookShoppingCart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookShoppingCart.Ripository
{
    public class CartRepository:ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepository(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {

            _context = applicationDbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }


        public bool CreateUserCart()
        {
            string userId = getUserId();
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User is not logged in");
                
            }
            ShoppingCart cart =  _context.ShoppingCart.FirstOrDefault(x => x.UserId == userId);
            if (cart != null)
            {
                return true;
            }
            else
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                };
                _context.ShoppingCart.Add(cart);
                _context.SaveChanges();
                return true;
            }
            
        }


        public int AddItem(int bookId, int qty)
        {
            string userId = getUserId();
            bool ans=  CreateUserCart();
            if( ans)
            {
                try
                {

               
                var cart= _context.ShoppingCart.FirstOrDefault(x => x.UserId == userId);
                if(cart != null)
                {

                
                    CartDetail cartItem = _context.CartDetail.FirstOrDefault(x => x.ShoppingCart_Id == cart.Id && x.BookId == bookId);
                
                    if (cartItem != null)
                    {
                        cartItem.Quantity += qty;
                        _context.SaveChanges();
                    }
                    else
                    {
                        cartItem = new CartDetail
                        {
                            BookId = bookId,
                            ShoppingCart_Id = cart.Id,
                            ShoppingCart =  cart,
                            Quantity = qty,
                        };
                        _context.CartDetail.Add(cartItem);
                        _context.SaveChanges();
                    }
                }
                
                return 1;
                }
                catch(Exception ex)
                {
                    return -1;
                }
        }
         return 0;
            
            
        }



        public int RemoveItem(int bookId, int qty=1)
        {
            string userId = getUserId();
            
            try
            {

                if (string.IsNullOrEmpty(userId)) { throw new Exception("User not Logged in"); }
                var cart = _context.ShoppingCart.FirstOrDefault(x => x.UserId == userId);
                
                if (cart == null)
                {
                    throw new Exception("No Cart for the User");
                }
                CartDetail cartItem =  _context.CartDetail.FirstOrDefault(x => x.ShoppingCart_Id == cart.Id && x.BookId == bookId);
                if (cartItem == null)
                {
                    throw new Exception("No Item to be deleted");
                }
                else if (cartItem.Quantity == 1)
                {
                    _context.CartDetail.Remove(cartItem);
                    _context.SaveChanges();
                }
                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                    _context.CartDetail.Update(cartItem);
                    _context.SaveChanges();
                    if (cartItem.Quantity < 0)
                    {
                        _context.CartDetail.Remove(cartItem);
                        _context.SaveChanges();
                    }
                }

                _context.SaveChanges();
               
                
            }
            catch (Exception ex)
            {
                
            }
            return 1;

        }


        public int CountItem()
        {
            string userId = getUserId();
            ShoppingCart cart = (ShoppingCart)_context.ShoppingCart.Where(x => x.UserId == userId);
            int count = 0;
            if (cart != null)
            {
                count = _context.CartDetail.Count(x => x.ShoppingCart_Id == cart.Id);
            }
            return count;
        }

        public  ShoppingCart GetCart(string userId="")
        {
            userId=getUserId();
            var cart = _context.ShoppingCart.FirstOrDefault(x => x.UserId == userId);
            if (cart == null) { throw new Exception("No Cart found"); }
            return  cart;
        }

        public string getUserId()
        {
            ClaimsPrincipal user = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(user);
            return userId;

        }





        public ShoppingCart GetUserCart()
        {
            var userId = getUserId();
            if (string.IsNullOrEmpty(userId)) { throw new Exception("Invalid User"); }
            var shoppingCart = _context.ShoppingCart
                        .Include(a => a.CartDetails)
                        .ThenInclude(a => a.Book)
                        .ThenInclude(a => a.Genre)
                        .Where(a => a.UserId == userId).FirstOrDefault();
            return shoppingCart;

        }

        public int GetCartItemCount(string userId="")
        {

             userId = getUserId();
            if (string.IsNullOrEmpty(userId)) { throw new Exception("Invalid User"); }
            var shoppingCart =  _context.ShoppingCart
                        .Include(a => a.CartDetails)
                        .ThenInclude(a => a.Book)
                        .ThenInclude(a => a.Genre)
                        .Where(a => a.UserId == userId).FirstOrDefault();
            if(shoppingCart == null) { return 0; }
            int count= shoppingCart.CartDetails.Count();
            return count;
        }



        public  bool DocheckOut()
        {
            using var transaction=_context.Database.BeginTransaction();
            try
            {
                //move data from cartDetail to order and orderDetail then remove cart detail
                var userId=getUserId();
                if (string.IsNullOrEmpty(userId)) { throw new Exception("User is not logged in"); }
                ShoppingCart cart= _context.ShoppingCart.FirstOrDefault(x => x.UserId == userId);
                if (cart == null) { throw new Exception("Nothing in Cart"); }
                var cartDetail= _context.CartDetail
                                    .Where(a=>a.ShoppingCart_Id == cart.Id).ToList(); 
                if(cartDetail.Count()<1)
                {
                    throw new Exception("Cart is empty");
                }
                var order = new Order
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    OrderStatusId = 1 //pending
                };
                _context.Order.Add(order);
                _context.SaveChanges();
                foreach(var item in cartDetail)
                {
                    var book=_context.Book.Find(item.BookId);
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = book.Price,
                    };
                    _context.OrderDetail.Add(orderDetail);
                }
                _context.SaveChanges();

                //removing cartDetail
                _context.CartDetail.RemoveRange(cartDetail);
                _context.SaveChanges(true);
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

      

    }
}

using BookShoppingCart.Models;

namespace BookShoppingCart
{
    public interface ICartRepository
    {
        int AddItem(int bookId, int qty);
        int RemoveItem(int bookId, int qty);
        int CountItem();
        string getUserId();
        ShoppingCart GetCart(string userId="");
        ShoppingCart GetUserCart();
        int GetCartItemCount(string userId = "");
        bool DocheckOut();


    }
}
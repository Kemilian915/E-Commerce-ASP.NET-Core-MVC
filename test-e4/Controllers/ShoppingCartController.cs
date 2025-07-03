using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_e4.Data;
using test_e4.Models;
using test_e4.ViewModels;

namespace test_e4.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly AppDbContext _context;
        private List<ShoppingCartItem> _cartItems;
        public ShoppingCartController(AppDbContext context)
        {
            _context = context;
            _cartItems = new List<ShoppingCartItem>();
        }
        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
            ViewBag.CartItemCount = cartItems.Sum(i => i.Quantity);

            return View();
        }
        [Authorize]
        public IActionResult ViewCart()
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            var cartViewModel = new ShoppingCartViewModel
            {
                CartItems = cartItems,
                TotalPrice = cartItems.Sum(item => item.Product?.Price * item.Quantity ?? 0)
            };
            return View(cartViewModel);
        }
        public IActionResult RemoveItem(int id)
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            var itemToRemove = cartItems.FirstOrDefault(item => item.Product.Id == id);

            if (itemToRemove.Quantity > 1)
            {
                itemToRemove.Quantity--;
            }
            else
            {
                cartItems.Remove(itemToRemove);
            }

            HttpContext.Session.Set("Cart", cartItems);
            return RedirectToAction("ViewCart");
        }
        public IActionResult PurchaseItems()
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            foreach (var item in cartItems)
            {
                _context.Purchases.Add(new Purchase
                {
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    PurchaseDate = DateTime.Now,
                    Total = item.Product.Price * item.Quantity
                });
            }
            _context.SaveChanges();

            HttpContext.Session.Set("Cart", new List<ShoppingCartItem>());

            return RedirectToAction("Index", "Home");
        }
    }
}

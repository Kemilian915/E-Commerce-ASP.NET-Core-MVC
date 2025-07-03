using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_e4.BusinessLayer.Interfaces;
using test_e4.Data;
using test_e4.Models;

namespace test_e4.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsServices _service;
        private readonly AppDbContext _context;

        public ProductsController(IProductsServices service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();
            ViewBag.CartItemCount = cartItems.Sum(i => i.Quantity);

            var allProducts = await _service.GetAllAsync();
            return View(allProducts);
        }
        //Get: Products/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name, Price, Camera, Storage, Battery, Color, Description, ImageURL")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            await _service.AddAsync(product);
            return RedirectToAction(nameof(Index));
        }

        //Get: Products/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            if (productDetails == null)
            {
                return View("NotFound");
            }
            return View(productDetails);
        }
        //Get: Products/Edit{id}
        public async Task<IActionResult> Edit(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            if (productDetails == null)
            {
                return View("NotFound");
            }
            return View(productDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price, Camera, Storage, Battery, Color, Description, ImageURL")] Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            await _service.UpdateAsync(id, product);
            return RedirectToAction(nameof(Index));
        }

        //Get: Products/Delete{id}
        public async Task<IActionResult> Delete(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            if (productDetails == null)
            {
                return View("NotFound");
            }
            return View(productDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productDetails = await _service.GetByIdAsync(id);
            if (productDetails == null)
            {
                return View("NotFound");
            }
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddProductToCart(int id)
        {
            var productToAdd = _context.Products.Find(id);

            var cartItems = HttpContext.Session.Get<List<ShoppingCartItem>>("Cart") ?? new List<ShoppingCartItem>();

            var existingCartItem = cartItems.FirstOrDefault(item => item.Product.Id == id);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                cartItems.Add(new ShoppingCartItem
                {
                    Product = productToAdd,
                    Quantity = 1
                });
            }

            HttpContext.Session.Set("Cart", cartItems);

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
using test_e4.Models;

namespace test_e4.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartItem> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public int CartItemCount => CartItems?.Sum(item => item.Quantity) ?? 0;

    }
}

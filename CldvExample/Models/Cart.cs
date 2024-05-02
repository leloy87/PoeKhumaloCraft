using System.ComponentModel.DataAnnotations;

namespace KhumaloeApp.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public string UserId { get; set; } // Foreign key to identify the user
        public ProductUser User { get; set; } // Navigation property
        public List<CartItem> CartItem { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

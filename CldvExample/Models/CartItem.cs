namespace KhumaloeApp.Models
{
    public class CartItem
    {
            public int CartItemId { get; set; }
            public int CartId { get; set; } // Foreign key to identify the shopping cart
            public Cart Cart { get; set; } // Navigation property
            public int KhProductId { get; set; } // Foreign key to identify the product
            public KhProducts KhProducts { get; set; } // Navigation property
            public int Quantity { get; set; }
       
    }
}

﻿using System.ComponentModel.DataAnnotations;

namespace KhumaloeApp.Models

{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; } // Foreign key to identify the product
        public KhProducts KhProducts { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}

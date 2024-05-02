using KhumaloP24.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhumaloP24.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }
}
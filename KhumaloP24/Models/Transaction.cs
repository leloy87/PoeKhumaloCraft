using KhumaloP24.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhumaloP24.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        public int? UserID { get; set; }

        public int? ProductID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? TransactionDate { get; set; }

        public int? Quantity { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? TotalAmount { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }

        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}

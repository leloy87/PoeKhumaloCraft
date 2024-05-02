using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhumaloP24.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string ImageURL { get; set; }

        [Required]
        public string Category { get; set; }

        public bool Availability { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }
    }
}
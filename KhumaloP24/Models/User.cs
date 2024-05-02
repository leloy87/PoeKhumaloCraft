
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KhumaloP24.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string UserType { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;

namespace KhumaloeApp.Models
{
    public class ProductUser: IdentityUser
    {
        public Cart Cart { get; set; }

    }
}

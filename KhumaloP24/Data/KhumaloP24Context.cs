using Microsoft.EntityFrameworkCore;
using KhumaloP24.Models;


namespace KhumaloP24.Data
{
    public class KhumaloP24Context : DbContext
    {
        public KhumaloP24Context(DbContextOptions<KhumaloP24Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Order> Orders { get; set; }

        // Add other DbSet properties for additional entities if needed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}

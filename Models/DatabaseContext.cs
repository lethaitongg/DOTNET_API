using Microsoft.EntityFrameworkCore;

namespace DEMO_BUOI07_API.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasMany(product => product.ProductImages).WithOne(image => image.Product).HasForeignKey(image => image.ProductId);
            });
        }

        public DbSet<Product> Products { get; set;}

        public DbSet<ProductImage> ProductImages { get; set;}
    }
}

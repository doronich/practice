using System;
using System.Threading;
using System.Threading.Tasks;
using ClothingStore.Data.Entities;
using ClothingStore.Data.Entities.Categories;
using ClothingStore.Data.Entities.item;
using ClothingStore.Data.Entities.Order;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Repository.Context {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :
            base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CouponCode> CouponCodes { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CouponCode>().HasIndex(u=>u.Code).IsUnique();
            modelBuilder.Entity<FavoriteItem>(item => {
                item.Property(o => o.UserId).IsRequired();
            });
            modelBuilder.Entity<User>().HasIndex(u => u.Login).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<SubCategory>()
                        .HasOne(s => s.Category)
                        .WithMany(c => c.SubCategories)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItem>()
                        .HasOne(i => i.Order)
                        .WithMany(o => o.OrderItems)
                        .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges() {
            this.ModifiedInformation();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) {
            this.ModifiedInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ModifiedInformation() {
            foreach(var entityEntry in this.ChangeTracker.Entries()) {
                if(!(entityEntry.Entity is BaseEntity entity)) continue;
                var now = DateTime.UtcNow;
                switch(entityEntry.State) {
                    case EntityState.Added:
                        entity.CreatedDate = now;
                        break;
                    case EntityState.Modified:
                        entity.UpdatedDate = now;
                        break;
                }
            }
        }
    }
}

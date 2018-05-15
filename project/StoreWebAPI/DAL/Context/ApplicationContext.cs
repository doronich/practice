using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context {
    public class ApplicationContext : DbContext {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        //    //optionsBuilder.UseSqlServer(@"Server=.\\SQLEXPRESS;Database=store;Trusted_Connection=True;");
        //}
    }
}

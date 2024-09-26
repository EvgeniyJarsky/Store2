using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Category> Category { get; set; }
		public DbSet<ApplicationType> ApplicationType { get; set; }
        public DbSet<Product> Product { get; set; }


    }
}

using Microsoft.EntityFrameworkCore;

namespace Expense_manager.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext( DbContextOptions options):base(options)
        { 
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
    }
}

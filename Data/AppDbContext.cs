using Microsoft.EntityFrameworkCore;
using Todo_Application.Model;

namespace Todo_Application.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TodoItems> TodoItems { get; set; }
        public DbSet<User> User { get; set; }
    }
}

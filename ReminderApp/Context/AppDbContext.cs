using Microsoft.EntityFrameworkCore;
using ReminderApp.Entities;

namespace ReminderApp.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
    }
}

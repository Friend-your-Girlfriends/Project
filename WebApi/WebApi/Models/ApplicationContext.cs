using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Instalation> Instalations { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

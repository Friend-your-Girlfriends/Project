using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

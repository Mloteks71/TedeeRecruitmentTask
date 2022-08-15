using Microsoft.EntityFrameworkCore;
using Tedee.Models;

namespace Tedee
{
    public class TedeeContext : DbContext
    {
        public TedeeContext(DbContextOptions<TedeeContext> options)
            : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<RegisteredEmail> RegisteredEmails { get; set; } = null!;
    }
}

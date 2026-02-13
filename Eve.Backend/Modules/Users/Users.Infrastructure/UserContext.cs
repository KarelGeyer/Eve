using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities;

namespace Users.Infrastructure
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<UserIdentity> UserIdentity { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<GDPR> UserGDPR { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
    }
}

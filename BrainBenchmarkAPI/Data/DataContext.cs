using Microsoft.EntityFrameworkCore;

namespace BrainBenchmarkAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserDb> Users { get; set; }
        public DbSet<BlacklistTokenDb> BlacklistTokens { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDb>().HasKey(x => x.Id);
            modelBuilder.Entity<UserDb>().ToTable("users");

            modelBuilder.Entity<BlacklistTokenDb>().HasKey(x => x.Token);
            modelBuilder.Entity<BlacklistTokenDb>().ToTable("blacklistTokens");

            base.OnModelCreating(modelBuilder);
        }
    }
}

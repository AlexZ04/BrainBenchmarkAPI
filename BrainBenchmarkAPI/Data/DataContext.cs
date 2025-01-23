using Microsoft.EntityFrameworkCore;

namespace BrainBenchmarkAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserDb> Users { get; set; }
        public DbSet<BlacklistTokenDb> BlacklistTokens { get; set; }
        public DbSet<AdminDb> AdminList { get; set; }
        public DbSet<GameDb> Games { get; set; }
        public DbSet<AttemptDb> Attempts { get; set; }
        public DbSet<SavedAttempt> SavedAttempts { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDb>().HasKey(x => x.Id);
            modelBuilder.Entity<UserDb>().ToTable("users");

            modelBuilder.Entity<BlacklistTokenDb>().HasKey(x => x.Token);
            modelBuilder.Entity<BlacklistTokenDb>().ToTable("blacklistTokens");

            modelBuilder.Entity<AdminDb>().HasKey(x => x.Id);
            modelBuilder.Entity<AdminDb>().ToTable("adminsList");

            modelBuilder.Entity<GameDb>().HasKey(x => x.Id);
            modelBuilder.Entity<GameDb>().ToTable("games");

            modelBuilder.Entity<AttemptDb>().HasKey(x => x.Id);
            modelBuilder.Entity<AttemptDb>().ToTable("attempts");

            modelBuilder.Entity<SavedAttempt>().HasKey(x => x.Id);
            modelBuilder.Entity<SavedAttempt>().ToTable("savedAttempts");

            base.OnModelCreating(modelBuilder);
        }
    }
}

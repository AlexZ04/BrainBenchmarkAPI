using Microsoft.EntityFrameworkCore;

namespace BrainBenchmarkAPI.Data
{
    public class DataContext : DbContext
    {
        public DbSet<UserDb> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDb>().HasKey(x => x.Id);
            modelBuilder.Entity<UserDb>().ToTable("users");

            base.OnModelCreating(modelBuilder);
        }
    }
}

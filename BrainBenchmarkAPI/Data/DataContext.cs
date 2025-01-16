using Microsoft.EntityFrameworkCore;

namespace BrainBenchmarkAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}

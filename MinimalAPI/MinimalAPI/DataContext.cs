using Microsoft.EntityFrameworkCore;
using MinimalAPI;

namespace HeroAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<SuperHero> SuperHeroes { get; set; }
    }
}

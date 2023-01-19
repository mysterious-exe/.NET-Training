using Microsoft.EntityFrameworkCore;
using Verify.Models;

namespace Verify.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }



        public DbSet<User> Users { get; set; }
    }
}

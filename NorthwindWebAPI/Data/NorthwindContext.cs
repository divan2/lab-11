// Data/NorthwindContext.cs
using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;

namespace NorthwindApi.Data
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
    }
}

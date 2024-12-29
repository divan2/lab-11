using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace StockServer
{
    public class StockContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost,1436;Database=StocksDB;User Id=SA;Password=LEGO1111;TrustServerCertificate=True;");
        }

        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        public StockContext() : base() { }
    }
}
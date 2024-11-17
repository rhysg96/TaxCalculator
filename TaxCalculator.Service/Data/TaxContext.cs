using Microsoft.EntityFrameworkCore;
using TaxCalculator.Models.Entities;

namespace TaxCalculator.Service.Data
{
    public class TaxContext : DbContext
    {
        public TaxContext(DbContextOptions<TaxContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaxBand>()
                .Property(e => e.TaxBandId)
                .ValueGeneratedOnAdd();
        }

        public DbSet<TaxBand> TaxBands { get; set; }
    }
}

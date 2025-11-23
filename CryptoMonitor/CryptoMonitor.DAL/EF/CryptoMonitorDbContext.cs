using System.Diagnostics; // Nodig voor Debug.WriteLine
using CryptoMonitor.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CryptoMonitor.DAL.EF
{
    public class CryptoMonitorDbContext : DbContext
    {
        public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<UserReview> UserReviews { get; set; }

        public CryptoMonitorDbContext()
        {
        }

        public CryptoMonitorDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .LogTo(message => Debug.WriteLine(message), LogLevel.Information); 
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeListing>()
                .HasKey(el => new { el.CryptocurrencyId, el.ExchangeId });

            modelBuilder.Entity<ExchangeListing>()
                .HasOne(el => el.Cryptocurrency)
                .WithMany(c => c.Listings)
                .HasForeignKey(el => el.CryptocurrencyId);

            modelBuilder.Entity<ExchangeListing>()
                .HasOne(el => el.Exchange)
                .WithMany(e => e.Listings)
                .HasForeignKey(el => el.ExchangeId);

            modelBuilder.Entity<UserReview>()
                .HasOne(r => r.Exchange)
                .WithMany(e => e.Reviews)
                .IsRequired();
        }

        public bool CreateDatabase(bool dropDatabase)
        {
            if (dropDatabase)
            {
                Database.EnsureDeleted();
            }
            return Database.EnsureCreated();
        }
    }
}
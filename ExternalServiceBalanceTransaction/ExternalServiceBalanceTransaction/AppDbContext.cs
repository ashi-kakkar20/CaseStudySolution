using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using ExternalServiceBalanceTransaction.Models;

namespace ExternalServiceBalanceTransaction
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<BalanceInformation> BalanceInformation { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BalanceInformation>().HasKey(u => u.UserId);
        }
    }
}


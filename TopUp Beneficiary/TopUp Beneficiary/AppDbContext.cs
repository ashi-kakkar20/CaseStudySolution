using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using TopUp_Beneficiary.Models;

namespace TopUp_Beneficiary
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<TopUpBeneficiary> TopUpBeneficiary { get; set; }
        public DbSet<TopUpTransaction> TopUpTransaction { get; set; }
        public DbSet<TopUpOption> TopUpOption { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasKey(u => u.Id);
            modelBuilder.Entity<TopUpBeneficiary>().HasKey(t => t.TopUpBeneficiaryId);
            modelBuilder.Entity<TopUpTransaction>().HasKey(t => t.TopUpTransactionId);
            modelBuilder.Entity<TopUpOption>().HasKey(t => t.TopUpOptionId);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FlatsAPI.Entities
{
    public class FlatsDbContext : DbContext
    {
        private readonly IConfiguration Configuration;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BlockOfFlats> BlockOfFlats { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Role> Roles { get; set; }
        public FlatsDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rent>()
                .HasMany(r => r.Tenants)
                .WithMany(a => a.Rents);

            modelBuilder.Entity<Rent>()
                .HasOne(r => r.Owner)
                .WithMany(o => o.OwnerShips);

            modelBuilder.Entity<Rent>()
                .HasOne(r => r.Flat)
                .WithMany(f => f.Rents)
                .IsRequired();

            modelBuilder.Entity<BlockOfFlats>()
                .HasMany(b => b.Flats)
                .WithOne(f => f.BlockOfFlats);

            modelBuilder.Entity<Flat>()
                .HasOne(f => f.BlockOfFlats)
                .WithMany(b => b.Flats)
                .HasForeignKey(f => f.BlockOfFlatsId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Configuration["ConnectionStrings:Default"]);
        }
    }
}

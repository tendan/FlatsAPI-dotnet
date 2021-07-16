using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<Permission> Permissions { get; set; }
        //public FlatsDbContext() { }
        public FlatsDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<BlockOfFlats>()
                .HasMany(b => b.Flats)
                .WithOne(f => f.BlockOfFlats);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Accounts)
                .WithOne(a => a.Role);*/

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .HasMany(a => a.Rents)
                .WithOne(r => r.RentIssuer);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.OwnedFlats)
                .WithOne(f => f.Owner);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.OwnedBlocksOfFlats)
                .WithOne(b => b.Owner);

            modelBuilder.Entity<Account>()
                .HasMany(a => a.RentedFlats)
                .WithMany(f => f.Tenants);

            modelBuilder.Entity<Rent>()
                .HasOne(r => r.RentIssuer)
                .WithMany(a => a.Rents)
                .IsRequired();

            modelBuilder.Entity<Rent>()
                .HasOne(r => r.FlatProperty)
                .WithMany(p => p.Rents);

            modelBuilder.Entity<Rent>()
                .HasOne(r => r.BlockOfFlatsProperty)
                .WithMany(p => p.Rents);

            modelBuilder.Entity<Permission>()
                .HasMany(p => p.Roles)
                .WithMany(r => r.Permissions);

            modelBuilder.Entity<Flat>()
                .HasOne(f => f.BlockOfFlats)
                .WithMany(b => b.Flats)
                .IsRequired();

            modelBuilder.Entity<Flat>()
                .HasOne(f => f.Owner)
                .WithMany(a => a.OwnedFlats);

            modelBuilder.Entity<Flat>()
                .HasMany(f => f.Rents)
                .WithOne(r => r.FlatProperty);

            modelBuilder.Entity<BlockOfFlats>()
                .HasMany(b => b.Rents)
                .WithOne(r => r.BlockOfFlatsProperty);

            modelBuilder.Entity<BlockOfFlats>()
                .HasOne(f => f.Owner)
                .WithMany(a => a.OwnedBlocksOfFlats);

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:Default"]);
        }
    }
}

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
        public DbSet<Permission> Permissions { get; set; }
        public FlatsDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rent>()
                .HasOne(r => r.RentIssuer)
                .WithMany(a => a.Rents)
                .IsRequired();

            modelBuilder.Entity<BlockOfFlats>()
                .HasMany(b => b.Flats)
                .WithOne(f => f.BlockOfFlats);

            modelBuilder.Entity<Flat>()
                .HasOne(f => f.BlockOfFlats)
                .WithMany(b => b.Flats)
                .HasForeignKey(f => f.BlockOfFlatsId)
                .IsRequired();

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Accounts)
                .WithOne(a => a.Role);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.Role)
                .WithMany(r => r.Accounts)
                .HasForeignKey(a => a.RoleId)
                .IsRequired();

            modelBuilder.Entity<Permission>()
                .HasMany(p => p.Roles)
                .WithMany(r => r.Permissions);

            modelBuilder.Entity<Flat>()
                .HasOne(f => f.Owner)
                .WithMany(a => a.OwnedFlats)
                .HasForeignKey(f => f.OwnerId)
                .IsRequired(false);

            modelBuilder.Entity<BlockOfFlats>()
                .HasOne(b => b.Owner)
                .WithMany(a => a.OwnedBlocksOfFlats)
                .HasForeignKey(f => f.OwnerId)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Configuration["ConnectionStrings:Default"]);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class FactoryContext : DbContext
    {
        public FactoryContext(DbContextOptions<FactoryContext> options) : base(options)
        {
        }

        public DbSet<LineModel> Lines{ get; set; }
        public DbSet<BuggyModel> Buggies { get; set; }
        public DbSet<RouteModel> Routes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LineModel>().ToTable("Line");
            modelBuilder.Entity<BuggyModel>().ToTable("Buggy");
            modelBuilder.Entity<RouteModel>().ToTable("Route");
        }
    }
}

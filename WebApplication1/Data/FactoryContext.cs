﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.ViewModels;

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
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ViewModel> ViewModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LineModel>().ToTable("Line");
            modelBuilder.Entity<BuggyModel>().ToTable("Buggy");
            modelBuilder.Entity<RouteModel>().ToTable("Route");
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder
                .Entity<ViewModel>(
            eb =>
            {
                eb.HasNoKey();
            });
        }
    }
}

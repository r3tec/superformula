using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PolicyAPI.Data.Models;

namespace PolicyAPI.Data
{
    public class PolicyContext : DbContext
    {
        public string DbPath { get; }

        public PolicyContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "Policy.sqlite");
        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
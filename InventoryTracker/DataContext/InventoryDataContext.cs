using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InventoryTracker.DataContext
{
    public class InventoryDataContext : DbContext
    {
        private readonly string _connectionString;

        public InventoryDataContext()
        {
            
        }

        public InventoryDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Division> Divisions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString ?? @"Data Source=(localdb)\ProjectsV13;Initial Catalog=InventoryDb;");
        }
    }
}

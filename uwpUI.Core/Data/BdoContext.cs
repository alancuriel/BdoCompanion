using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using uwpUI.Core.Models;

namespace uwpUI.Core.Data
{
    public class BdoContext : DbContext
    {
        public DbSet<BdoItem> Items { get; set; }
        public DbSet<ItemGroup> ItemGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bdoData.db");
        }
    }
}

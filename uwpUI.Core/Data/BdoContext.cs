using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using uwpUI.Core.Models;

namespace uwpUI.Core.Data
{
    public class BdoContext : DbContext
    {
        public DbSet<BdoItemModel> Items { get; set; }
        public DbSet<ItemGroupModel> ItemGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bdoData.db");
        }
    }
}

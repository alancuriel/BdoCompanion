using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uwpUI.Core.Data;
using uwpUI.Core.Models;

namespace uwpUI.Core.Services
{
    public static class BdoDataService
    {
        private static BdoContext db;
        public static async Task InitializeAsync()
        {
            await Task.CompletedTask;
            db = new BdoContext();
        }

        public static BdoItem GetItemById(int id)
        {
            return db.Items.Find(id);
        }

        public static BdoItem DeleteItem(int id)
        {
            BdoItem item = GetItemById(id);

            if(item != null)
            {
                db.Items.Remove(item);
            }

            return item;
        }

        public static IEnumerable<BdoItem> AllItems()
        {
            return db.Items.ToList();
        }

        public static BdoItem AddItem(BdoItem item)
        {
            db.Add(item);
            return item;
        }

        public static int Commit()
        {
            return db.SaveChanges();
        }

        public static BdoItem UpdateItem(BdoItem updatedItem)
        {
            var entity = db.Items.Attach(updatedItem);
            entity.State = EntityState.Modified;
            return updatedItem;
        }

    }
}

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

        public static IEnumerable<BdoItem> GetItemsByKnowledge(string knowledge)
        {
            IEnumerable<BdoItem> query = from i in db.Items
                            where i.Knowledge.StartsWith(knowledge) || string.IsNullOrEmpty(knowledge)
                            select i;
            return query;
        }

        public static ItemGroup AddItemGroup(ItemGroup itemGroup)
        {
            db.Add(itemGroup);
            return itemGroup;
        }

        public static ItemGroup UpdateItemGroup(ItemGroup updatedItemGroup)
        {
            var entity = db.ItemGroups.Attach(updatedItemGroup);
            entity.State = EntityState.Modified;
            return updatedItemGroup;
        }

        public static ItemGroup GetItemGroupById(int i)
        {
            return db.ItemGroups.Find(i);
        }

        public static IEnumerable<ItemGroup> AllItemGroups()
        {
            return db.ItemGroups.ToList();
        }
    }
}

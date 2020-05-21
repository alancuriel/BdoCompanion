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

        public static IEnumerable<Recipe> AllCookingRecipes()
        {
            IEnumerable<Recipe> query = from r in db.Recipes
                           where r.Type == RecipeType.Cooking
                           select r;
            return query;
        }

        public static IEnumerable<RecipeMat> GetRecipeMatsByRecipeID(int recipeId)
        {
            IEnumerable<RecipeMat> query = from rm in db.RecipeMats
                                           where rm.RecipeId == recipeId
                                           select rm;
            return query;
        }

        public static IEnumerable<Recipe> AllAlchemyRecipes()
        {
            IEnumerable<Recipe> query = from r in db.Recipes
                                        where r.Type == RecipeType.Alchemy
                                        select r;
            return query;
        }

        public static bool RecipeMatExists(RecipeMat recipeMat) => db.RecipeMats.Contains(recipeMat);

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

        public static IEnumerable<Recipe> AllRecipes() => db.Recipes.ToList();

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

        public static Recipe AddRecipe(Recipe recipe)
        {
            db.Add(recipe);
            return recipe;
        }

        public static RecipeMat AddRecipeMat(RecipeMat recipeMat)
        {
            db.Add(recipeMat);
            return recipeMat;
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

        public static BdoItem FirstItemFromItemGroup(int itemGroupId)
        {
            BdoItem query = (from ig in db.Items
                             where ig.ItemGroupId == itemGroupId
                             select ig).First();
            return query;
        }

        public static IEnumerable<ItemGroup> AllItemGroups()
        {
            return db.ItemGroups.ToList();
        }

        public static Recipe DeleteRecipe(int id)
        {
            Recipe recipe = GetRecipeById(id);

            if (recipe != null)
            {
                db.Recipes.Remove(recipe);
            }

            return recipe;
        }

        public static Recipe GetRecipeById(int id)
        {
            return db.Recipes.Find(id);
        }

        public static IEnumerable<Recipe> GetCookingRecipeByName(string str)
        {
            IEnumerable<Recipe> query = from i in db.Recipes
                                        where i.Type == RecipeType.Cooking &&
                                        (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)||
                                        i.Name.ToLower().Contains(str.ToLower()))
                                        select i;
            return query;
        }

        public static IEnumerable<Recipe> GetAlchemyRecipeByName(string str)
        {
            IEnumerable<Recipe> query = from i in db.Recipes
                                        where i.Type == RecipeType.Alchemy &&
                                        (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) ||
                                        i.Name.ToLower().Contains(str.ToLower()))
                                        select i;
            return query;
        }

        public static RecipeMat DeleteRecipeMat(int id)
        {
            RecipeMat recipeMat = GetRecipeMatById(id);

            if(recipeMat != null)
            {
                db.RecipeMats.Remove(recipeMat);
            }

            return recipeMat;
        }

        private static RecipeMat GetRecipeMatById(int id)
        {
            return db.RecipeMats.Find(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Caliburn.Micro;
using uwpUI.Core.Models;
using uwpUI.Core.Services;
using uwpUI.Helpers;

namespace uwpUI.ViewModels
{
    public class CookingRecipesViewModel : Conductor<CookingRecipesDetailViewModel>.Collection.OneActive
    {
        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync(RecipeType.Cooking);
        }

        public List<RecipeType> RecipeTypes { get; set; } = new List<RecipeType> { RecipeType.Cooking, RecipeType.Alchemy };


        public async Task LoadDataAsync(RecipeType recipeType)
        {


            Items.Clear();
            IEnumerable<Recipe> data;

            if (recipeType == RecipeType.Cooking)
            {
                data = BdoDataService.AllCookingRecipes();
                await Task.CompletedTask; 
            }
            else
            {
                data = BdoDataService.AllAlchemyRecipes();
                await Task.CompletedTask;
            }

            Items.AddRange(data.Select(d => new CookingRecipesDetailViewModel(d)));
        }
    }
}

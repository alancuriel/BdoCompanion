using System.Collections.Generic;
using System.Threading.Tasks;

using Caliburn.Micro;
using uwpUI.Core.Models;
using uwpUI.Core.Services;
using Windows.UI.Xaml.Input;

namespace uwpUI.ViewModels
{
    public class CookingRecipesViewModel : Screen /*Conductor<CookingRecipesDetailViewModel>.Collection.OneActive*/
    {
        public BindableCollection<Recipe> Recipes { get; set; } = new BindableCollection<Recipe>();
        private Recipe _selectedRecipe;

        public Recipe SelectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                Set(ref _selectedRecipe, value);
                //NotifyOfPropertyChange(() => SelectedRecipe);
            }
        }

        private string _searchStr;

        public string SearchStr
        {
            get
            {
                return _searchStr;
            }
            set
            {
                Set(ref _searchStr, value);
                NotifyOfPropertyChange(() => SearchStr);
            }
        }


        private RecipeType _currentRecipeType;

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync(RecipeType.Cooking);
        }

        public List<RecipeType> RecipeTypes { get; set; } = new List<RecipeType> { RecipeType.Cooking, RecipeType.Alchemy };


        public async Task LoadDataAsync(RecipeType recipeType)
        {
            _currentRecipeType = recipeType;
            Recipes.Clear();
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

            Recipes.AddRange(data);
            NotifyOfPropertyChange(() => Recipes);
        }

        public async void SearchRecipe()
        {
            Recipes.Clear();
            IEnumerable<Recipe> data;

            if (_currentRecipeType == RecipeType.Cooking)
            {
                data = BdoDataService.GetCookingRecipeByName(SearchStr);
                await Task.CompletedTask;
            }
            else
            {
                data = BdoDataService.GetAlchemyRecipeByName(SearchStr);
                await Task.CompletedTask;
            }

            Recipes.AddRange(data);
            NotifyOfPropertyChange(() => Recipes);
        }

        public void EnterSearchRecipe(KeyRoutedEventArgs args)
        {
            if (args.Key == Windows.System.VirtualKey.Enter) SearchRecipe();
        }
            

    }
}

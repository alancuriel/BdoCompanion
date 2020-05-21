using System.Collections.Generic;
using System.Threading.Tasks;

using Caliburn.Micro;
using uwpUI.Core.Models;
using uwpUI.Core.Services;
using uwpUI.Models;
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
                if (_selectedRecipe != value )
                {
                    Set(ref _selectedRecipe, value);
                    if(value != null) LoadMaterials();
                }
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

        private BindableCollection<RecipeMaterialModel> _materials = new BindableCollection<RecipeMaterialModel>();
        public BindableCollection<RecipeMaterialModel> Materials
        {
            get
            {
                return _materials;
            }
            set
            {
                _materials = value;
                NotifyOfPropertyChange(() => Materials);
            }
        }

        private BindableCollection<BdoItem> _results = new BindableCollection<BdoItem>();

        public BindableCollection<BdoItem> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                NotifyOfPropertyChange(() => Results);
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

        private void LoadMaterials()
        {
            Materials.Clear();
            Results.Clear();
            var materials = BdoDataService.GetRecipeMatsByRecipeID(SelectedRecipe.Id);
            foreach (var material in materials)
            {
                if (material.IsItem)
                {
                    material.Item = BdoDataService.GetItemById(material.ItemId.Value);
                    Materials.Add(new RecipeMaterialModel
                    {
                        Name = material.Item.Name,
                        Img = material.Item.Img,
                        Grade = material.Item.Grade,
                        IsItem = true,
                        Id = material.Item.Id,
                        Amount = material.Amount
                    });
                }
                else
                {
                    //Item Group
                    material.ItemGroup = BdoDataService.GetItemGroupById(material.ItemGroupId.Value);
                    BdoItem item = BdoDataService.FirstItemFromItemGroup(material.ItemGroupId.Value);
                    Materials.Add(new RecipeMaterialModel
                    {
                        Name = material.ItemGroup.Name,
                        Img = material.ItemGroup.Items[0].Img,
                        Grade = ItemGrade.White,
                        IsItem = false,
                        Id = material.ItemGroup.Id,
                        Amount = material.Amount
                    });
                }
            }

            BdoItem result0 = BdoDataService.GetItemById(SelectedRecipe.Item1Id.Value);
            Results.Add(result0);

            if (SelectedRecipe.Item2Id.HasValue)
            {
                BdoItem result1 = BdoDataService.GetItemById(SelectedRecipe.Item2Id.Value);
                Results.Add(result1);
            }

            NotifyOfPropertyChange(() => Materials);
            NotifyOfPropertyChange(() => Results);
        }


    }
}
